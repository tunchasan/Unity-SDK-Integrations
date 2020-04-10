using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Library.Purchasing 
{
    public class AndroidIAP : IStoreListener
    {
        // Items list, configurable via inspector
        private List<CatalogItem> Catalog;

        // The Unity Purchasing system
        private static IStoreController m_StoreController;

        // We are initialized when StoreController and Extensions are set and we are logged in
        public bool IsInitialized
        {
            get
            {
                return m_StoreController != null && Catalog != null;
            }
        }

        // Get Store Items List
        public List<CatalogItem> GetCatalogItems()
        {
            return Catalog;
        }

        public void OnGUI() // FOR TEST
        {
            // This line just scales the UI up for high-res devices
            // Comment it out if you find the UI too large.
            GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(3, 3, 3));

            // if we are not initialized, only draw a message
            if (!IsInitialized)
            {
                Debug.LogError("You must logged in to Playfab...");
                return;
            }

            // Draw menu to purchase items
            foreach (var item in Catalog)
            {
                if (GUILayout.Button("Buy " + item.DisplayName))
                {
                    // On button click buy a product
                    BuyProductID(item.ItemId);

                }
            }
        }

        /// <summary>
        /// ///Initialize CatalogItems before show up Store Page.
        /// </summary>

        public void InitializeIAPItems()
        {
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), result => {

                Catalog = result.Catalog;

                // Make UnityIAP initialize
                InitializePurchasing();

            }, error => Debug.LogError(error.GenerateErrorReport()));
        }

        // This is invoked manually on Start to initialize UnityIAP
        private void InitializePurchasing()
        {
            // If IAP is already initialized, return gently
            if (IsInitialized) return;

            // Create a builder for IAP service
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));

            // Register each Consumable item from the catalog
            foreach (var item in Catalog)
            {
                builder.AddProduct(item.ItemId, ProductType.Consumable);
            }

            // Trigger IAP service initialization
            UnityPurchasing.Initialize(this, builder);
        }

        // This is automatically invoked automatically when IAP service is initialized
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            m_StoreController = controller;
        }

        // This is automatically invoked automatically when IAP service failed to initialized
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }

        // This is automatically invoked automatically when purchase failed
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }

        // This is invoked automatically when successful purchase is ready to be processed
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            // NOTE: this code does not account for purchases that were pending and are
            // delivered on application start.
            // Production code should account for such case:
            // More: https://docs.unity3d.com/ScriptReference/Purchasing.PurchaseProcessingResult.Pending.html

            if (!IsInitialized)
            {
                return PurchaseProcessingResult.Complete;
            }

            // Test edge case where product is unknown
            if (e.purchasedProduct == null)
            {
                Debug.LogWarning("Attempted to process purchase with unknown product. Ignoring");
                return PurchaseProcessingResult.Complete;
            }

            // Test edge case where purchase has no receipt
            if (string.IsNullOrEmpty(e.purchasedProduct.receipt))
            {
                Debug.LogWarning("Attempted to process purchase with no receipt: ignoring");
                return PurchaseProcessingResult.Complete;
            }

            Debug.Log("Processing transaction: " + e.purchasedProduct.transactionID);

            // Deserialize receipt
            var googleReceipt = GooglePurchase.FromJson(e.purchasedProduct.receipt);

            // Invoke receipt validation
            // This will not only validate a receipt, but will also grant player corresponding items
            // only if receipt is valid.
            PlayFabClientAPI.ValidateGooglePlayPurchase(new ValidateGooglePlayPurchaseRequest()
            {
                // Pass in currency code in ISO format
                CurrencyCode = e.purchasedProduct.metadata.isoCurrencyCode,
                // Convert and set Purchase price
                PurchasePrice = (uint)(e.purchasedProduct.metadata.localizedPrice * 100),
                // Pass in the receipt
                ReceiptJson = googleReceipt.PayloadData.json,
                // Pass in the signature
                Signature = googleReceipt.PayloadData.signature
            }, result => Debug.Log("Validation successful!"),
               error => Debug.Log("Validation failed: " + error.GenerateErrorReport())
            );

            return PurchaseProcessingResult.Complete;
        }

        // This is invoked manually to initiate purchase
        private void BuyProductID(string productId)
        {
            // If IAP service has not been initialized, fail hard
            if (!IsInitialized) throw new Exception("IAP Service is not initialized!");

            // Pass in the product id to initiate purchase
            m_StoreController.InitiatePurchase(productId);
        }
    }

    // The following classes are used to deserialize JSON results provided by IAP Service
    // Please, note that JSON fields are case-sensitive and should remain fields to support Unity Deserialization via JsonUtilities
    public class JsonData
    {
        // JSON Fields, ! Case-sensitive

        public string orderId;
        public string packageName;
        public string productId;
        public long purchaseTime;
        public int purchaseState;
        public string purchaseToken;
    }

    public class PayloadData
    {
        public JsonData JsonData;

        // JSON Fields, ! Case-sensitive
        public string signature;
        public string json;

        public static PayloadData FromJson(string json)
        {
            var payload = JsonUtility.FromJson<PayloadData>(json);
            payload.JsonData = JsonUtility.FromJson<JsonData>(payload.json);
            return payload;
        }
    }

    public class GooglePurchase
    {
        public PayloadData PayloadData;

        // JSON Fields, ! Case-sensitive
        public string Store;
        public string TransactionID;
        public string Payload;

        public static GooglePurchase FromJson(string json)
        {
            var purchase = JsonUtility.FromJson<GooglePurchase>(json);
            purchase.PayloadData = PayloadData.FromJson(purchase.Payload);
            return purchase;
        }
    }
}
