using Library.Purchasing;
using System;
using UnityEngine;
using UnityEngine.Monetization;

public class PurchaseExample : MonoBehaviour
{
    private AndroidIAP _androidIAP;

    private void Awake()
    {
        _androidIAP = new AndroidIAP();

        _androidIAP.OnIAPServicesInitialized += OnServiceInitializeSucceed;

        _androidIAP.OnIAPServicesInitializeFailed += OnServiceInitializeFailed;

        _androidIAP.OnPurchasesValidationSucceed += OnValidationSucceed;

        _androidIAP.OnPurchasesValidationFailed += OnValidationFailed;

        _androidIAP.OnPurchasesSucceed += OnPurchaseSucceed;

        _androidIAP.OnPurchasesFailed += OnPurchaseFailed;
    }

    private void OnPurchaseFailed(UnityEngine.Purchasing.Product product, string error)
    {
        Debug.LogError("OnPurchaseFailed! : " + error);
    }

    private void OnPurchaseSucceed(UnityEngine.Purchasing.Product product)
    {
        throw new NotImplementedException();
    }

    private void OnValidationFailed(UnityEngine.Purchasing.Product product, string error)
    {
        Debug.LogError("OnValidationFailed! : " + error);
    }

    private void OnValidationSucceed(UnityEngine.Purchasing.Product product)
    {
        throw new NotImplementedException();
    }

    private void OnServiceInitializeFailed(string error)
    {
        Debug.LogError("OnServiceInitializeFailed! : " + error);
    }

    private void OnServiceInitializeSucceed()
    {
        // SERVICES ARE READY TO PURCHASE PROCESS

        _androidIAP.BuyProduct("PRODUCT_WEAPON");
    }

    void Start()
    {
        _androidIAP.InitializeIAPItems(

            (actionSuccess) =>
            {
                // actionSuccess -> LIST OF YOUR MARKET ITEMS

                // YOU CAN INITIALIZE PURCHASING
                _androidIAP.InitializePurchasingServices(AndroidIAP.ItemType.OnlyConsumable);
            },

            (actionFailure) =>
            {
                // SOMETHING WRONG, YOU CAN'T HANDLE ANY PURHCASING PROCESS
            });

    }

}
