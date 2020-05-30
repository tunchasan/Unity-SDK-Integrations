using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Library.Purchasing
{
    public class IAPurchase : MonoBehaviour, IStoreListener
    {
        private static IStoreController m_StoreController;

        private static IExtensionProvider m_StoreExtensionProvider;

        // Step 1 Create your products
        private static string removeAds = "removeADS";

        void Start()
        {
            InitializePurchasing();
        }

        public void InitializePurchasing()
        {
            if (IsInitialized()) { return; }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(removeAds, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }

        // Step 2 choose if the product is a consumable or non consumable

        private bool IsInitialized()
        {
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        // Step 3 Create methods
        public void BuyRemoveAds()
        {
            BuyProductID(removeAds);
        }

        public void BuyProductID(string productId)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Product product = m_StoreController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            if(String.Equals(args.purchasedProduct.definition.id, removeAds, StringComparison.Ordinal)){

                Debug.Log("Purchase succeed.");
            }
            else
            {
                Debug.Log("Purchase Failed");
            }

            return PurchaseProcessingResult.Complete;
        }














        public void OnInitializeFailed(InitializationFailureReason error)
        {
            throw new NotImplementedException();
        }

        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            throw new NotImplementedException();
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            throw new NotImplementedException();
        }
    }
}
    
