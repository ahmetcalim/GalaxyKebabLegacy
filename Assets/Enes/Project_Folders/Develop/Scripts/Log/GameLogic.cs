using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class GameLogic : MonoBehaviour
{

    public List<Customer> customers;
    public List<Ingredient> ingredients;
    public Transform viewport;
    public OrderItem oItem;
    public OrderControl oControl;  
    public GameObject star;   
    bool isPlay;
    public int playingTime=300;
    public int interval = 20;
    int timeCounter = 0;
    GameObject currentOrderPrefab;
    public static bool hasOrder;
    List<Order> orders=new List<Order>();
    Taste result;
    Order currentOrder;
    Popularity popularity;
    Session session;
    double spawnRate;
    public HoverButton hoverButton;
    public LavasGenerator lavasGenerator;
    public void Start()
    {
        if (!isPlay)
        {
            lavasGenerator.GenerateLavas();
            isPlay = true;
            popularity = new Popularity();
            popularity.Activate();
            session = new Session();
            session.Activate();
            spawnRate = 0.5f + 0.005f * popularity.globalPopularity;
            StartCoroutine(RecursiveCounter());
        }
    }
    public void PauseGame()
    {
        isPlay = false;
    }
    public void EndGame()
    {
        FinishOrder();
        isPlay = false;
        popularity.SetGlobalPopularity();
        Debug.Log("Oyun Bitti!");       
    }
    IEnumerator RecursiveCounter()
    {

        if (isPlay)
        {
            if (timeCounter < playingTime)
            {
                if (timeCounter==0)
                {
                    CreateOrder(customers[Random.Range(0, customers.Count)]);
                }
                else
                {                
                    if (spawnRate >= 1)
                    {
                        for (int i = 0; i < int.Parse(spawnRate.ToString("0.##").Split(',')[0]); i++)
                            CreateOrder(customers[Random.Range(0, customers.Count)]);

                        if (Random.Range(0.00f, 1.00f) < float.Parse("0," + spawnRate.ToString("0.##").Split(',')[1]))
                            CreateOrder(customers[Random.Range(0, customers.Count)]);
                    }
                    else
                    {
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
            averageTasteRatingnValue =_customer.averageTasteRatingnValue,
            customerName = _customer.customerName,
            personality = new Personality { counterActive = _customer.personality.counterActive, impatianceValue = _customer.personality.impatianceValue, orderTime = _customer.personality.orderTime },
            Tastes = new List<Taste>(_customer.Tastes)

        };

        Order order = new Order(customer, ingredients,2);
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
        }
        else
        {
            orders[orders.Count - 2].nextOrder = order;
        }
    }
    int customerIndex;
    void SetCustomer()
    {
        if (orders.Count>customerIndex+1)
        {
            currentOrder = currentOrder.nextOrder;
            currentOrderPrefab = currentOrder.orderPrefab;
            currentOrder.customer.personality.SetCounter(true);
            StartCoroutine(currentOrder.customer.personality.TimeCounter(this));
            currentOrderPrefab.GetComponent<OrderItem>().SetColor(Color.yellow);
            oControl.SetValues(currentOrder.customer, currentOrder.customer.Tastes);
            customerIndex++;
        }
        else
        {
            hasOrder = false;
            Debug.Log("Elinde sipariş yok.");
        }
    }
    string percentage;
    int kalan;
    public void FinishOrder()
    {
        if (RepomaticBehaviour.canThrow)
        {
            if (!currentOrder.isFinished)
            {
                hoverButton.enabled = false;
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
                currentOrder.customer.averageTasteRatingnValue = 0;
                currentOrder.customer.personality.SetCounter(false);
            }
            SetCustomer();
        }
          
    }
    public void AddIngredient(int ingredientID)
    {
        for (int i = 0; i < ingredients[ingredientID].tastes.Count; i++)
        {
            result = currentOrder.customer.Tastes.Where(t => t.taste == ingredients[ingredientID].tastes[i].taste).ToList().FirstOrDefault();
            if (result.totalInputCount<=100)
            {
                switch (result.preference)
                {
                    case Taste.Preference.irrelevant:
                        result.totalInputCount += ingredients[ingredientID].tastes[i].tasteInput;
                        if (result.totalInputCount > 70)
                            result.CalculateAverageTasteRating(Satisfaction.CalculateIrrelevantSatisfaction(result.totalInputCount));
                        break;
                    case Taste.Preference.like:
                        result.totalInputCount += ingredients[ingredientID].tastes[i].tasteInput;
                        result.CalculateAverageTasteRating(Satisfaction.CalculateSatisfaction(result.x_max, result.x_zero, result.totalInputCount, 1));
                        break;
                    case Taste.Preference.dislike:
                        result.totalInputCount += ingredients[ingredientID].tastes[i].tasteInput;
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


}



