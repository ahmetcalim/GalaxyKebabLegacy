using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{

    public List<Customer> customers;
    public List<Ingredient> ingredients;
    public Transform viewport;
    public OrderItem oItem;
    public OrderControl oControl;
    Order currentOrder;
    GameObject currentOrderPrefab;
    public GameObject star;
    Taste result;
    bool isPlay;
    public static int playingTime = 0;
    public static int interval = 20;
    List<Order> orders=new List<Order>();

    public void StartGame()
    {
        if (!isPlay)
        {
            isPlay = true;
            Popularity.Instance();
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
        Popularity.SetGlobalPopularity();
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
                foreach (Taste taste in currentOrder.customer.Tastes)
                {
                    if (taste.totalInputCount == 0)
                    {
                        if (taste.isLike)
                            taste.tasteRating = -1;
                        else
                            taste.tasteRating = 1;
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
                        Debug.Log("percentage: " + percentage);
                        Debug.Log("kalan: " + kalan);
                        if (percentage.Length == 1)
                        {
                            percentage += 0;
                        }
                        Debug.Log("tam yıldız sayısı" + (int.Parse(percentage) - kalan));
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
                Debug.Log("average: " + currentOrder.customer.averageTasteRatingnValue);
                currentOrder.isFinished = true;
                Popularity.CalculateDailyPopularity(currentOrder.customer.averageTasteRatingnValue * currentOrder.customer.personality.impactFactor);
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
                if (result != null)
                {
                    result.totalInputCount += ingredients[ingredientID].tastes[i].tasteInput;
                    result.CalculateAverageTasteRating(Satisfaction.CalculateSatisfaction(result.x_max, result.x_zero, result.totalInputCount, result.isLike == true ? 1 : -1));
                }
            }
        currentOrder.customer.CalculateAverageSatisfactionValue();
        oControl.SetValues(currentOrder.customer,currentOrder.customer.Tastes);
    }

   
}



