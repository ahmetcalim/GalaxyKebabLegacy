using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class GameLogic : MonoBehaviour
{

    public List<Customer> customers;
    public Transform customerSpawnPoint;
    public List<Ingredient> ingredients;
    public Transform viewport;
    public OrderItem oItem;
    public OrderControl oControl;
    public SummaryView summaryView;
    public GameObject star;
    bool isPlay;
    public int playingTime = 300;
    public int interval = 20;
    public int timeCounter = 0;
    GameObject currentOrderPrefab;
    List<Order> orders = new List<Order>();
    Taste result;
    public Order currentOrder;
    Popularity popularity;
    Session session;
    public static bool hasOrder;
    double spawnRate;

    public LavasGenerator lavasGenerator;

    public IngredientSorter ingredientSorter;
    public void Start()
    {
        if (!isPlay)
        {
          
            SetIrrelevantFunction();
            lavasGenerator.GenerateLavas();
            isPlay = true;
            popularity = new Popularity();
            popularity.Activate();
            session = new Session();
            session.Activate();
            spawnRate = 0.5f + 0.005f * popularity.globalPopularity;
            summaryView.globalFirst.text = popularity.averageDailyPopularity.ToString();
            StartCoroutine(RecursiveCounter());
            ingredientSorter.CheckTasteLights();
        }
    }
    public void PauseGame()
    {
        isPlay = false;
    }
    public void EndGame()
    {
        isPlay = false;
        FinishOrder();
        summaryView.transform.gameObject.SetActive(true);
        summaryView.totalOrder.text = orders.Count.ToString();
        summaryView.dailyRating.text = (Popularity.DailyPopularity.dailyPopularity / Popularity.DailyPopularity.index).ToString();
        summaryView.successOrder.text = successOrderCount.ToString();
        popularity.SetGlobalPopularity();
        summaryView.globalLast.text = popularity.averageDailyPopularity.ToString();
        Debug.Log("Oyun Bitti!");
        successOrderCount = 0;
    }
    IEnumerator RecursiveCounter()
    {

        if (isPlay)
        {
            if (timeCounter < playingTime)
            {
                if (timeCounter != 1000)
                {
                    CreateOrder(customers[Random.Range(0, customers.Count)]);
                    //Debug.Log("BAŞLANGIÇ");
                }
                else
                {
                    if (spawnRate >= 1)
                    {
                        Debug.Log("spawn rate 1'den BÜYÜK");

                        for (int i = 0; i < int.Parse(spawnRate.ToString("0.##").Split(',')[0]); i++)
                            CreateOrder(customers[Random.Range(0, customers.Count)]);

                        if (Random.Range(0.00f, 1.00f) < float.Parse("0," + spawnRate.ToString("0.##").Split(',')[1]))
                            CreateOrder(customers[Random.Range(0, customers.Count)]);
                    }
                    else
                    {
                        Debug.Log("spawn rate 1'den KÜÇÜK");
                        if (Random.Range(0.00f, 1.00f) < spawnRate)
                            CreateOrder(customers[Random.Range(0, customers.Count)]);
                    }
                }

                timeCounter += interval;
                if (currentOrder.isFinished)
                    SetCustomer();
            }
            else
                EndGame();
            yield return new WaitForSeconds(interval);
            StartCoroutine(RecursiveCounter());
        }
        else
            yield break;
    }
    void CreateOrder(Customer _customer)
    {
        hasOrder = true;
        Customer customer = new Customer
        {
            averageTasteRatingnValue = _customer.averageTasteRatingnValue,
            customerName = _customer.customerName,
            model = _customer.model,
            personality = new Personality { counterActive = _customer.personality.counterActive, orderTime = _customer.personality.orderTime,irrelevantFunction = _customer.personality.irrelevantFunction },
            Tastes = new List<Taste>(_customer.Tastes)

        };

        Order order = new Order(customer, ingredients, 3);
        orders.Add(order);
        oItem.ClearTexts();
        for (int i = 0; i < order.finalIngredients.Count; i++)
            oItem.ingredients[i].text = order.finalIngredients[i].ingredientName;
        oItem.customerName.text = order.customer.customerName;
        order.orderPrefab = Instantiate(oItem.prefab, viewport);

        if (orders.Count == 1)
        {
            currentOrder = orders[0];
            currentOrderPrefab = currentOrder.orderPrefab;
            currentOrder.customer.personality.SetCounter(true);
            StartCoroutine(currentOrder.customer.personality.TimeCounter(this));
            currentOrderPrefab.GetComponent<OrderItem>().SetColor(Color.yellow);
            oControl.SetValues(currentOrder.customer, currentOrder.customer.Tastes);
            customer.model = Instantiate(customer.model, customerSpawnPoint.position, Quaternion.identity);
            customer.model.transform.localRotation = Quaternion.Euler(customer.model.transform.localRotation.x, customer.model.transform.localRotation.y + 180f, customer.model.transform.localRotation.z);
        }
        else
        {
            orders[orders.Count - 2].nextOrder = order;
            orders[orders.Count - 2].nextOrder.customer.model = Instantiate(orders[orders.Count - 2].nextOrder.customer.model, new Vector3(orders[orders.Count - 2].customer.model.transform.position.x, orders[orders.Count - 2].customer.model.transform.position.y, orders[orders.Count - 2].customer.model.transform.position.z + 8), Quaternion.identity);
            orders[orders.Count - 2].nextOrder.customer.model.transform.localRotation = Quaternion.Euler(orders[orders.Count - 2].nextOrder.customer.model.transform.localRotation.x, orders[orders.Count - 2].nextOrder.customer.model.transform.localRotation.y + 180f, orders[orders.Count - 2].nextOrder.customer.model.transform.localRotation.z);
        }
        currentOrder.customer.model.GetComponent<CustomerBehaviour>().isActive = true;

    }
    int customerIndex;
    void SetCustomer()
    {
        if (orders.Count > customerIndex + 1)
        {
            currentOrder.nextOrder.prevOrder = currentOrder;
            currentOrder = currentOrder.nextOrder;
            currentOrderPrefab = currentOrder.orderPrefab;
            currentOrder.customer.personality.SetCounter(true);
            StartCoroutine(currentOrder.customer.personality.TimeCounter(this));
            currentOrderPrefab.GetComponent<OrderItem>().SetColor(Color.yellow);
            oControl.SetValues(currentOrder.customer, currentOrder.customer.Tastes);
            customerIndex++;
            SetModelPosition();

            currentOrder.customer.model.GetComponent<CustomerBehaviour>().isActive = true;
        }
        else
        {
            CreateOrder(customers[Random.Range(0, customers.Count)]);
            SetCustomer();
        }
    }
    int successOrderCount;
    string percentage;
    int kalan;
    void SetModelPosition()
    {
        for (int i = 0; i < orders.Count; i++)
        {
            orders[i].customer.model.transform.position = new Vector3(orders[i].customer.model.transform.position.x, orders[i].customer.model.transform.position.y, orders[i].customer.model.transform.position.z - 8);
        }
    }
    public void FinishOrder()
    {
       
        if (RepomaticBehaviour.canThrow)
        {
            
            if (!currentOrder.isFinished)
            {
                
                foreach (Taste taste in currentOrder.customer.Tastes)
                {
                    if (taste.totalInputCount == 0)
                    {
                        switch (taste.preference)
                        {
                            case Taste.Preference.like:
                                taste.tasteRating = -1;
                                break;
                            case Taste.Preference.dislike:
                                taste.tasteRating = 1;
                                break;
                            default:
                                break;
                        }
                    }
                }
                currentOrder.customer.CalculateAverageSatisfactionValue();
                double iFactor = Satisfaction.CalculateImpactFactor(currentOrder.customer.personality.orderTime, Random.Range(50, 70), popularity.averageDailyPopularity, 1, 1);
                currentOrder.customer.averageTasteRatingnValue *= iFactor;
                Debug.Log("Average: " + currentOrder.customer.averageTasteRatingnValue);
                popularity.CalculateDailyPopularity(currentOrder.customer.averageTasteRatingnValue);
                if (currentOrder.customer.averageTasteRatingnValue > 0)
                {
                    if (currentOrder.customer.averageTasteRatingnValue >= 1)
                    {
                        for (int i = 0; i < 10; i++)
                            Instantiate(star, currentOrderPrefab.GetComponent<OrderItem>().satisfaction.transform);
                    }
                    else
                    {
                        percentage = currentOrder.customer.averageTasteRatingnValue.ToString("0.##").Split(',')[1];
                        kalan = int.Parse(percentage) % 10;
                        if (percentage.Length == 1)
                        {
                            percentage += 0;
                        }
                        for (int i = 0; i < (int.Parse(percentage) - kalan) / 10; i++)
                            Instantiate(star, currentOrderPrefab.GetComponent<OrderItem>().satisfaction.transform);
                        if (kalan != 0)
                        {
                            GameObject star1 = Instantiate(star, currentOrderPrefab.GetComponent<OrderItem>().satisfaction.transform);
                            star1.GetComponent<Image>().fillAmount = (float)kalan / 10.0f;
                        }
                    }
                    currentOrderPrefab.GetComponent<OrderItem>().SetColor(Color.green);
                    successOrderCount++;
                }
                else
                {
                    Debug.Log("Hiç yıldız alamadım");
                    currentOrderPrefab.GetComponent<OrderItem>().SetColor(Color.red);
                }

                foreach (Taste taste in currentOrder.customer.Tastes)
                {
                    taste.tasteRating = 0;
                    taste.totalInputCount = 0;
                }
                currentOrder.isFinished = true;
                currentOrder.customer.model.SetActive(false);
                currentOrder.customer.averageTasteRatingnValue = 0;
                currentOrder.customer.personality.SetCounter(false);
            }
            SetCustomer();
            ingredientSorter.CheckTasteLights();
        }
    }
    public void AddIngredient(int ingredientID)
    {
        for (int i = 0; i < ingredients[ingredientID].tastes.Count; i++)
        {
            result = currentOrder.customer.Tastes.Where(t => t.taste == ingredients[ingredientID].tastes[i].taste.taste).ToList().FirstOrDefault();
            if (result.totalInputCount <= 100)
            {
                switch (result.preference)
                {
                    case Taste.Preference.irrelevant:
                        result.totalInputCount += ingredients[ingredientID].tastes[i].tasteInput;
                        if (currentOrder.customer.personality.irrelevantFunction)
                        {
                            if (result.totalInputCount > 45)
                                result.CalculateAverageTasteRating(Satisfaction.CalculateIrrelevantSatisfaction_SweetBump(result.totalInputCount));
                        }
                        else
                        {
                            if (result.totalInputCount > 70)
                                result.CalculateAverageTasteRating(Satisfaction.CalculateIrrelevantSatisfaction_OverTaste(result.totalInputCount));
                        }

                        break;
                    case Taste.Preference.like:
                        result.totalInputCount += ingredients[ingredientID].tastes[i].tasteInput;
                        result.CalculateAverageTasteRating(Satisfaction.CalculateSatisfaction(result.x_max, result.x_zero, result.totalInputCount, 1));
                        break;
                    case Taste.Preference.dislike:
                        result.totalInputCount += ingredients[ingredientID].tastes[i].tasteInput;
                        if (result.totalInputCount <= result.x_max)
                            result.CalculateAverageTasteRating(Satisfaction.CalculateSatisfaction(result.x_max, result.x_zero, result.totalInputCount, -1));
                        break;
                    default:
                        break;
                }
            }
        }
        currentOrder.customer.CalculateAverageSatisfactionValue();
        oControl.SetValues(currentOrder.customer, currentOrder.customer.Tastes);
    }
    void SetIrrelevantFunction()
    {
        foreach (Customer item in customers)
        {
            if (Random.Range(0, 2) == 1)
                item.personality.irrelevantFunction = true;
            else
                item.personality.irrelevantFunction = false;
        }
    }

}



