﻿using UnityEngine;
using System.Collections;
using CloudGoods.SDK.Models;
using CloudGoods.SDK.Container;

namespace CloudGoods.SDK.Container
{

    public class SwapItemAddAction : MonoBehaviour, IContainerAddAction
    {
        // which index of the container items will be swapped (default first item)
        public int swapIndex = 0;

        //The Amount of items needed in container until it is required to swap
        public int swapItemLimit = 0;

        private ItemContainer itemContainer;

        void Awake()
        {
            itemContainer = GetComponent<ItemContainer>();
        }

        public void AddItem(OwnedItemInformation addItem, int amount, bool isSave)
        {
            if (IsSwapNeeded())
            {
                //Should only swap single item in container (first item in container items list)
                OwnedItemInformation swapItem = itemContainer.containerItems[swapIndex];

                ItemContainerManager.MoveItem(swapItem, addItem.OwnerContainer);
                AddItemToContainer(addItem, amount, isSave);
            }
            else
            {
                AddItemToContainer(addItem, amount, isSave);
            }
        }

        void AddItemToContainer(OwnedItemInformation addItem, int amount, bool isSave)
        {
            if (amount == -1)
            {
                amount = addItem.Amount;
                addItem.OwnerContainer = itemContainer;

                itemContainer.containerItems.Add(addItem);
                itemContainer.AddItemEvent(addItem, isSave);

            }
            else
            {
                addItem.Amount = amount;
                addItem.OwnerContainer = itemContainer;

                itemContainer.containerItems.Add(addItem);
                itemContainer.AddItemEvent(addItem, isSave);
            }
        }

        bool IsSwapNeeded()
        {
            if (itemContainer.containerItems.Count >= swapItemLimit)
                return true;
            else
                return false;
        }


    }

}