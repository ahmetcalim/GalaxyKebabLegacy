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
    int playingTime = 0;
    public int interval = 30;
    GameObject currentOrderPrefab;
    List<Order> orders=new List<Order>();
    Taste result;
    Order currentOrder;
    Popularity popularity;
    Session session;
    public AlienSpawn alienSpawn;
    public HoverButton hoverButton;
    public void StartGame()
    {
        if (!isPlay)
        {

            isPlay = true;
            popularity = new Popularity();
            popularity.Activate();
            session = new Session();
            session.Activate();
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
            if (playingTime < 300)
            {
                CreateOrder(customers[Random.Range(0, customers.Count)]);
                playingTime += interval;
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
        alienSpawn.SpawnCustomer();
        Order order = new Order(_customer,ingredients,2);
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
            currentOrderPrefab.GetComponent<OrderItem>().SetColor(Color.yellow);
            oControl.SetValues(currentOrder.customer, currentOrder.customer.Tastes);
            customerIndex++;
        }
        else
        {
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
                RepomaticBehaviour.canThrow = false;
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
                popularity.CalculateDailyPopularity(currentOrder.customer.averageTasteRatingnValue * currentOrder.customer.personality.impactFactor);
                Debug.Log("Average: " + currentOrder.customer.averageTasteRatingnValue);
                currentOrder.customer.averageTasteRatingnValue = 0;
            }
        }
        SetCustomer();       
    }

    public void AddIngredient(int ingredientID)
    {
        for (int i = 0; i < ingredients[ingredientID].tastes.Count; i++)
        {
            result = currentOrder.customer.Tastes.Where(t => t.taste == ingredients[ingredientID].tastes[i].taste).ToList().FirstOrDefault();

            if (result.totalInputCount<=1000)
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



