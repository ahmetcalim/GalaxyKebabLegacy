using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public List<Customer> customers;
    public List<Ingredient> ingredients;
    public List<GameObject> alienPrefabs;
    public Transform customerSpawnPoint;
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
    CustomerIngredient result;
    public Order currentOrder;
    Popularity popularity;
    Session session;
    SessionItem sessionItem;
    public static bool hasOrder;
    public LavasGenerator lavasGenerator;
    public Test test;
    double spawnRate;

    public void Start()
    {
        ClearAllIngredientValues();
        if (!isPlay)
        {
            isPlay = true;
            lavasGenerator.GenerateLavas();
            StartPopularity();
            StartSummaryView();
            StartSession();
            StartCoroutine(RecursiveCounter());
        }
    }
    public void EndGame()
    {
        isPlay = false;
        FinishOrder();
        FinishSession();
        FinishSummaryView();
        FinishPopularity();
        summaryView.globalLast.text = popularity.averageDailyPopularity.ToString();
        successOrderCount = 0;
        Debug.Log("Oyun Bitti!");
    }
    void ClearAllIngredientValues()
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            ingredients[i].rating = 0;
            ingredients[i].totalCost = 0;
            ingredients[i].totalInputCount = 0;

        }
    }
    void StartPopularity()
    {
        popularity = new Popularity();
        popularity.Activate();
        spawnRate = 0.5f + 0.5f * popularity.averageDailyPopularity;
    }
    void StartSummaryView()
    {
        summaryView.globalFirst.text = popularity.averageDailyPopularity.ToString();
    }
    void StartSession()
    {
        session = new Session();
        sessionItem = new SessionItem();
        session.Activate();
        session.sessionsItems.Add(sessionItem);

    }
    void FinishSummaryView()
    {
        summaryView.transform.gameObject.SetActive(true);
        summaryView.totalOrder.text = orders.Count.ToString();
        summaryView.dailyRating.text = (Popularity.DailyPopularity.dailyPopularity / Popularity.DailyPopularity.index).ToString();
        summaryView.successOrder.text = successOrderCount.ToString();
        summaryView.Print();
        summaryView.averageCost.text = summaryView.f_averageCost.ToString("0.##") + "$";
        summaryView.totalCost.text = summaryView.f_totalCost.ToString("0.##") + "$";

    }
    void FinishPopularity()
    {
        popularity.SetGlobalPopularity();
    }
    void FinishSession()
    {
        sessionItem.dayOfPlay = popularity.kAct + 1;
        sessionItem.dailyPopularity = popularity.averageDailyPopularity;
        sessionItem.CalculateAverageCostOfSession();
        session.SetGlobalPopularity();
    }
    IEnumerator RecursiveCounter()
    {

        if (isPlay)
        {
            if (timeCounter < playingTime)
            {
                if (timeCounter == 0)
                {
                    CreateOrder(customers[Random.Range(0, customers.Count)]);
                }

                else
                {
                    if (spawnRate >= 1)
                    {
                        Debug.Log("spawn rate 1'den BÜYÜK");

                        for (int i = 0; i < int.Parse(spawnRate.ToString("0.##").Split(',')[0]); i++)
                            CreateOrder(customers[Random.Range(0, customers.Count)]);

                        if (Random.Range(0.00f, 1.00f) < float.Parse("0," + spawnRate.ToString("0.##").Split('.')[1]))
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
        CustomerIngredient meat = _customer.c_Ingredients.Where(i => i.preference == CustomerIngredient.Preference.meat).FirstOrDefault();
        meat.x_zero = Random.Range(15, 25) + (int)(popularity.averageDailyPopularity * 10);
        meat.x_max = Random.Range(35, 45) + (int)(popularity.averageDailyPopularity * 20);
        hasOrder = true;
        Customer customer = new Customer
        {
            averageRating = _customer.averageRating,
            customerName = _customer.customerName,
            model = _customer.model,
            personality = new Personality { counterActive = _customer.personality.counterActive, orderTime = _customer.personality.orderTime },
            c_Ingredients = new List<CustomerIngredient>()
        };
        foreach (CustomerIngredient item in _customer.c_Ingredients)
        {
            customer.c_Ingredients.Add(new CustomerIngredient { ingredient = item.ingredient, x_zero = item.x_zero, x_max = item.x_max, inOrder = item.inOrder, preference = item.preference });
        }
        Order order = new Order(customer, Random.Range(2, 5));
        orders.Add(order);
        oItem.ClearTexts();
        for (int i = 0; i < order.finalIngredients.Count; i++)
            oItem.ingredients[i].text = order.finalIngredients[i].name;
        oItem.customerName.text = order.customer.customerName;
        order.orderPrefab = Instantiate(oItem.prefab, viewport);

        if (orders.Count == 1)
        {
            currentOrder = orders[0];
            currentOrderPrefab = currentOrder.orderPrefab;
            currentOrder.customer.personality.SetCounter(true);
            StartCoroutine(currentOrder.customer.personality.TimeCounter(this));
            currentOrderPrefab.GetComponent<OrderItem>().SetColor(Color.yellow);
            oControl.SetValues(currentOrder.customer, currentOrder.customer.c_Ingredients);
            customer.model = Instantiate(customer.model, customerSpawnPoint.position, Quaternion.identity);
            customer.model.transform.localRotation = Quaternion.Euler(customer.model.transform.localRotation.x, customer.model.transform.localRotation.y + 180f, customer.model.transform.localRotation.z);
            sessionItem.AddSessionItem();
        }
        else
        {
            orders[orders.Count - 2].nextOrder = order;
            orders[orders.Count - 2].nextOrder.customer.model = Instantiate(orders[orders.Count - 2].nextOrder.customer.model, new Vector3(orders[orders.Count - 2].customer.model.transform.position.x, orders[orders.Count - 2].customer.model.transform.position.y, orders[orders.Count - 2].customer.model.transform.position.z + 8), Quaternion.identity);
            orders[orders.Count - 2].nextOrder.customer.model.transform.localRotation = Quaternion.Euler(orders[orders.Count - 2].nextOrder.customer.model.transform.localRotation.x, orders[orders.Count - 2].nextOrder.customer.model.transform.localRotation.y + 180f, orders[orders.Count - 2].nextOrder.customer.model.transform.localRotation.z);
        }
        test.SetGradient3(currentOrder.customer.c_Ingredients.Where(i => i.preference == CustomerIngredient.Preference.meat).ToList().FirstOrDefault().x_max);
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
            oControl.SetValues(currentOrder.customer, currentOrder.customer.c_Ingredients);
            customerIndex++;
            SetModelPosition();
            sessionItem.AddSessionItem();
            currentOrder.customer.model.GetComponent<CustomerBehaviour>().isActive = true;
        }
        else
        {
            if (isPlay)
            {
                CreateOrder(customers[Random.Range(0, customers.Count)]);
                SetCustomer();
            }
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

        if (!currentOrder.isFinished)
        {

            foreach (CustomerIngredient c_Ingredient in currentOrder.customer.c_Ingredients)
            {
                if (c_Ingredient.ingredient.totalInputCount == 0)
                {
                    switch (c_Ingredient.preference)
                    {
                        case CustomerIngredient.Preference.like:
                            if (currentOrder.finalIngredients.Contains(c_Ingredient.ingredient))
                                c_Ingredient.ingredient.rating = -1;
                            else
                                c_Ingredient.ingredient.rating = 0;

                            break;
                        case CustomerIngredient.Preference.dislike:
                            c_Ingredient.ingredient.rating = 1;
                            break;
                        case CustomerIngredient.Preference.meat:
                            c_Ingredient.ingredient.rating = -1;
                            break;
                        default:
                            break;
                    }
                }
            }
            currentOrder.customer.CalculateAverageSatisfactionValue();
            double iFactor = Satisfaction.CalculateImpactFactor(currentOrder.customer.personality.orderTime, Random.Range(50, 70) - (int)(popularity.averageDailyPopularity * 10), popularity.averageDailyPopularity, 1, 1);
            currentOrder.customer.averageRating *= iFactor;
            Debug.Log("Average: " + currentOrder.customer.averageRating);
            popularity.CalculateDailyPopularity(currentOrder.customer.averageRating);
            if (currentOrder.customer.averageRating > 0)
            {
                if (currentOrder.customer.averageRating >= 1)
                {
                    for (int i = 0; i < 10; i++)
                        Instantiate(star, currentOrderPrefab.GetComponent<OrderItem>().satisfaction.transform);
                }
                else
                {
                    percentage = currentOrder.customer.averageRating.ToString("0.##").Split(',')[1];
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
                currentOrderPrefab.GetComponent<OrderItem>().SetColor(Color.red);
            }
            currentOrder.isFinished = true;
            currentOrder.customer.ClearCustomer();
            currentOrder.customer.model.SetActive(false);
            currentOrder.customer.personality.SetCounter(false);
            summaryView.CalculateOrderItems(sessionItem.sessionOrders[sessionItem.orderCount].orderIngredients);
        }
        SetCustomer();
    }

    public void AddIngredient(int ingredientID)
    {

        result = currentOrder.customer.c_Ingredients.Where(i => i.ingredient.ID == ingredientID).ToList().FirstOrDefault();
        if (result == null)
        {
            Ingredient irrelevantIngredient = ingredients.Where(i => i.ID == ingredientID).ToList().FirstOrDefault();
            if (!currentOrder.customer.irrelevantIngredients.Contains(irrelevantIngredient))
                currentOrder.customer.irrelevantIngredients.Add(irrelevantIngredient);

            irrelevantIngredient.totalInputCount += irrelevantIngredient.actionInput;
            if (irrelevantIngredient.totalInputCount >= 70 && irrelevantIngredient.totalInputCount <= 100)
            {
                irrelevantIngredient.CalculateAverageRating(Satisfaction.CalculateIrrelevantSatisfaction_OverTaste(irrelevantIngredient.totalInputCount));
                irrelevantIngredient.CalculateCost();
                sessionItem.sessionOrders[sessionItem.orderCount].AddSessionIngredient(irrelevantIngredient.ID, irrelevantIngredient.ingredientName, irrelevantIngredient.rating, irrelevantIngredient.totalCost, irrelevantIngredient.totalInputCount);
            }
        }
        else
        {
            if (result.ingredient.totalInputCount <= 100)
            {
                switch (result.preference)
                {
                    case CustomerIngredient.Preference.like:
                        result.ingredient.totalInputCount += result.ingredient.actionInput;
                        if (result.inOrder)
                        {
                            result.ingredient.CalculateAverageRating(Satisfaction.CalculateSatisfaction(result.x_max - (popularity.averageDailyPopularity * 10), result.x_zero + (popularity.averageDailyPopularity * 10), result.ingredient.totalInputCount, 1));
                            result.ingredient.CalculateCost();
                        }
                        else
                        {
                            result.ingredient.CalculateAverageRating(Satisfaction.CalculateSatisfaction(result.x_max - (popularity.averageDailyPopularity * 10), result.x_zero + (popularity.averageDailyPopularity * 10), result.ingredient.totalInputCount, 1) * 0.3f);
                            result.ingredient.CalculateCost();
                        }
                        break;
                    case CustomerIngredient.Preference.dislike:
                        result.ingredient.totalInputCount += result.ingredient.actionInput;
                        if (result.ingredient.totalInputCount <= result.x_max)
                        {
                            result.ingredient.CalculateAverageRating(Satisfaction.CalculateSatisfaction(result.x_max - (popularity.averageDailyPopularity * 10), result.x_zero + (popularity.averageDailyPopularity * 10), result.ingredient.totalInputCount, -1));
                            result.ingredient.CalculateCost();
                        }

                        break;
                    default:
                        break;

                }
                sessionItem.sessionOrders[sessionItem.orderCount].AddSessionIngredient(result.ingredient.ID, result.ingredient.ingredientName, result.ingredient.rating, result.ingredient.totalCost, result.ingredient.totalInputCount);
            }

        }
        currentOrder.customer.CalculateAverageSatisfactionValue();
        oControl.SetValues(currentOrder.customer, currentOrder.customer.c_Ingredients);
    }
    public void AddMeat(int ingredientID, float x, float y)
    {
        result = currentOrder.customer.c_Ingredients.Where(i => i.ingredient.ID == ingredientID).ToList().FirstOrDefault();
        if (result.ingredient.totalInputCount <= 100)
        {
            result.ingredient.totalInputCount += Mathf.Abs(x * y * 0.85f);
            if (result.ingredient.totalInputCount <= result.x_max)
            {
                result.ingredient.CalculateAverageRating(Satisfaction.CalculateSatisfactionMeat(result.x_max - (popularity.averageDailyPopularity * 10), result.x_zero + (popularity.averageDailyPopularity * 10), result.ingredient.totalInputCount, 1) * 3);
                result.ingredient.CalculateCost();
            }

            sessionItem.sessionOrders[sessionItem.orderCount].AddSessionIngredient(result.ingredient.ID, result.ingredient.ingredientName, result.ingredient.rating, result.ingredient.totalCost, result.ingredient.totalInputCount);
        }

        currentOrder.customer.CalculateAverageSatisfactionValue();
        oControl.SetValues(currentOrder.customer, currentOrder.customer.c_Ingredients);
    }
}



