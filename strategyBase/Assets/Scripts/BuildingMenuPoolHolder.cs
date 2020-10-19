using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BuildingMenuPoolHolder : MonoBehaviour
    {

        [SerializeField] private List<PooledButton> pooledMenuButtons;
        [SerializeField] private int desiredColumnCount = 2;


        private RectTransform _contentHolderRect;
        private ScrollRect _thisScrollRect;
        private Vector2 _resolution;
        private float _spacing;
        private float _buttonSize;
        private List<BuildingMenuPool> _menuPoolList;
        private SortedDictionary<int, List<PooledButton>> _buttonTable;
        private int _bottomIndex;
        private PooledButton _bottomButtons;
        private int _topIndex;
        private PooledButton _topButtons;
        private int _rows;
    

        private void Awake()
        {
            _buttonTable = new SortedDictionary<int, List<PooledButton>>();
            _menuPoolList = new List<BuildingMenuPool>();
        
            if (pooledMenuButtons.Count > 0)
            {
                foreach (var button in pooledMenuButtons)
                { 
                    BuildingMenuPool menuPool = new BuildingMenuPool(button);
                    _menuPoolList.Add(menuPool);

                }
            }
            else {DestroyImmediate(this);}

            _resolution = new Vector2(Screen.width, Screen.height);
            _thisScrollRect = GetComponent<ScrollRect>();
            _thisScrollRect.onValueChanged.AddListener(SliderMoved);


            _contentHolderRect = _thisScrollRect.content;


            ScreenResChangeCheck.OnScreenResChange += ScreenResChangeCheck_OnScreenResChange;

        }

        private void ScreenResChangeCheck_OnScreenResChange(object sender, ScreenResChangeCheck.OnScreenResChangeArgs e)
        {
       
       
        }


        private void Start()
        {
            CalculateDimensions();
            PlaceMenuButtons(_rows);
            CheckUpAndDownRowLimit();

        }

        private void CalculateDimensions()
        {
            float scrollWidth  = _thisScrollRect.gameObject.GetComponent<RectTransform>().sizeDelta.x;
            float scrollHeight = _resolution.y;
            _buttonSize = scrollWidth / (desiredColumnCount + 1);
            _spacing = _buttonSize / desiredColumnCount;
            _rows = Mathf.CeilToInt((scrollHeight / (_buttonSize + _spacing)) + 1);
        
        }

        public void SliderMoved(Vector2 vec2)
        {
            CheckUpAndDownRowLimit();
        }


        private void PlaceMenuButtons(int rows)
        {
            int buttonTypeIndex = 0;

            for (int i = -rows / 2; i <= rows-2 / 2; i++)
            {
                _buttonTable.Add(i,GetRow(i, buttonTypeIndex));
            }

        }


        private List<PooledButton> GetRow(int row,int poolIndex)
        {
            List<PooledButton> buttonRow = new List<PooledButton>();
            for (int j = 0; j < desiredColumnCount; j++)
            {

                buttonRow.Add(PlaceButton(row, j, _menuPoolList[poolIndex % (_menuPoolList.Count)]));
                poolIndex++;
            
            }

       
            return buttonRow;
        }

        private PooledButton PlaceButton(int i, int j,BuildingMenuPool pool)
        {
            PooledButton pooledButton = pool.Get();

            pooledButton.gameObject.SetActive(true);
            RectTransform rectTransform;
            (rectTransform = pooledButton.GetComponent<RectTransform>()).SetParent(_contentHolderRect);
            rectTransform.sizeDelta = new Vector2(_buttonSize, _buttonSize);
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition =
                new Vector2(
                    j * (_buttonSize + _spacing) + _spacing / 2 + _buttonSize /2,
                    i * (_buttonSize + _spacing)
                );

            pooledButton.MyPool = pool;
            return pooledButton;

        }

    


        private void CheckUpAndDownRowLimit()
        {
            GetTopAndBottomButtons();


            while (_bottomButtons.transform.position.y > -_resolution.y / 2)
            {
                AddRow(_bottomIndex - 1, _menuPoolList.IndexOf(_buttonTable[_bottomIndex][desiredColumnCount - 1].MyPool) + 1);
                GetTopAndBottomButtons();
            }

            while (_topButtons.transform.position.y < _resolution.y * 1.5f)
            {
                AddRow(_topIndex + 1, _menuPoolList.IndexOf(_buttonTable[_topIndex][desiredColumnCount - 1].MyPool) + 1);
                GetTopAndBottomButtons();
            }
            while (_topButtons.transform.position.y > _resolution.y * 2)
            {
                ReturnRowToPool(_topIndex);
                GetTopAndBottomButtons();
            }
            while (_bottomButtons.transform.position.y < -_resolution.y)
            {
                ReturnRowToPool(_bottomIndex);
                GetTopAndBottomButtons();
            }


        }

        private void GetTopAndBottomButtons()
        {
        
            _bottomIndex = _buttonTable.ElementAt(0).Key;
            _bottomButtons = _buttonTable[_bottomIndex][0];
            _topIndex = _buttonTable.ElementAt(_buttonTable.Count - 1).Key;
            _topButtons = _buttonTable[_topIndex][0];
        }

        private void AddRow(int row,int buttonPool)
        {
        
            _buttonTable.Add(row, GetRow(row, buttonPool));
        }

        private void ReturnRowToPool(int row)
        {
       
            foreach (PooledButton button in _buttonTable[row])
            {
                button.ReturnToPool();
            }

            _buttonTable.Remove(row);
        }

    }
}


