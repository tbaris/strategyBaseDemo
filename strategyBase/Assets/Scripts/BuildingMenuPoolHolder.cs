using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuPoolHolder : MonoBehaviour
{

    [SerializeField] private List<PooledButton> pooledMenuButtons;
    [SerializeField] private int desiredColumnCount = 2;


    private RectTransform contentHolderRect;
    private ScrollRect thisScrollRect;
    private Vector2 resolution;
    private float spacing = 0;
    private float buttonSize = 0;
    private List<BuildingMenuPool> menuPoolList;
    private SortedDictionary<int, List<PooledButton>> buttonTable;
    private int bottomIndex;
    private PooledButton bottomButtons;
    private int topIndex;
    private PooledButton topButtons;
    private int rows = 0;
    

    private void Awake()
    {
        buttonTable = new SortedDictionary<int, List<PooledButton>>();
        menuPoolList = new List<BuildingMenuPool>();
        
        if (pooledMenuButtons.Count > 0)
        {
            foreach (var button in pooledMenuButtons)
            { 
                BuildingMenuPool menuPool = new BuildingMenuPool(button);
                menuPoolList.Add(menuPool);

            }
        }
        else {DestroyImmediate(this);}

        resolution = new Vector2(Screen.width, Screen.height);
        thisScrollRect = GetComponent<ScrollRect>();
        thisScrollRect.onValueChanged.AddListener(sliderMoved);


        contentHolderRect = thisScrollRect.content;


        ScreenResChangeCheck.OnScreenResChange += ScreenResChangeCheck_OnScreenResChange;

    }

    private void ScreenResChangeCheck_OnScreenResChange(object sender, ScreenResChangeCheck.OnScreenResChangeArgs e)
    {
       
       
    }


    private void Start()
    {
        calculateDimensions();
        placeMenuButtons(rows);
        checkUpAndDownRowLimit();

    }

    private void calculateDimensions()
    {
        float scrollWidth  = thisScrollRect.gameObject.GetComponent<RectTransform>().sizeDelta.x;
        float scrollHeight = resolution.y;
        buttonSize = scrollWidth / (desiredColumnCount + 1);
        spacing = buttonSize / desiredColumnCount;
        rows = Mathf.CeilToInt((scrollHeight / (buttonSize + spacing)) + 1);
        
    }

    public void sliderMoved(Vector2 vec2)
    {
        checkUpAndDownRowLimit();
    }


    private void placeMenuButtons(int rows)
    {
        int buttonTypeIndex = 0;

        for (int i = -rows / 2; i <= rows-2 / 2; i++)
        {
            buttonTable.Add(i,getRow(i, buttonTypeIndex));
        }

    }


    private List<PooledButton> getRow(int row,int poolIndex)
    {
        List<PooledButton> buttonRow = new List<PooledButton>();
        for (int j = 0; j < desiredColumnCount; j++)
        {

            buttonRow.Add(placeButton(row, j, menuPoolList[poolIndex % (menuPoolList.Count)]));
            poolIndex++;
            
        }

       
        return buttonRow;
    }

    private PooledButton placeButton(int i, int j,BuildingMenuPool pool)
    {
        PooledButton pooledButton = pool.Get();

        pooledButton.gameObject.SetActive(true);
        RectTransform rectTransform;
        (rectTransform = pooledButton.GetComponent<RectTransform>()).SetParent(contentHolderRect);
        rectTransform.sizeDelta = new Vector2(buttonSize, buttonSize);
        rectTransform.localScale = Vector3.one;
        rectTransform.anchoredPosition =
            new Vector2(
                j * (buttonSize + spacing) + spacing / 2 + buttonSize /2,
                i * (buttonSize + spacing)
            );

        pooledButton.myPool = pool;
        return pooledButton;

    }

    


    private void checkUpAndDownRowLimit()
    {
        GetTopAndBottomButtons();


        while (bottomButtons.transform.position.y > -resolution.y / 2)
        {
            addRow(bottomIndex - 1, menuPoolList.IndexOf(buttonTable[bottomIndex][desiredColumnCount - 1].myPool) + 1);
            GetTopAndBottomButtons();
        }

        while (topButtons.transform.position.y < resolution.y * 1.5f)
        {
            addRow(topIndex + 1, menuPoolList.IndexOf(buttonTable[topIndex][desiredColumnCount - 1].myPool) + 1);
            GetTopAndBottomButtons();
        }
        while (topButtons.transform.position.y > resolution.y * 2)
        {
            returnRowToPool(topIndex);
            GetTopAndBottomButtons();
        }
        while (bottomButtons.transform.position.y < -resolution.y)
        {
            returnRowToPool(bottomIndex);
            GetTopAndBottomButtons();
        }


    }

    private void GetTopAndBottomButtons()
    {
        
        bottomIndex = buttonTable.ElementAt(0).Key;
        bottomButtons = buttonTable[bottomIndex][0];
        topIndex = buttonTable.ElementAt(buttonTable.Count - 1).Key;
        topButtons = buttonTable[topIndex][0];
    }

    private void addRow(int row,int buttonPool)
    {
        
        buttonTable.Add(row, getRow(row, buttonPool));
    }

    private void returnRowToPool(int row)
    {
       
        foreach (PooledButton button in buttonTable[row])
        {
            button.returnToPool();
        }

        buttonTable.Remove(row);
    }

}


