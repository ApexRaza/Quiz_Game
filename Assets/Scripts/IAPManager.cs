using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private static IAPManager instance;
    private IStoreController storeController;
    private IExtensionProvider storeExtensionProvider;

    // Define product IDs (match these with App Store IDs)
    public static string Gem100 = "gem_pack100";
    public static string Gem600 = "gem_pack600";
    public static string Gem1550 = "gem_pack1550";
    public static string Gem4650 = "gem_pack4650";
    public static string Gem12250 = "gem_pack12250";
    //public static string PRODUCT_NO_ADS = "no_ads";
   // public static string PRODUCT_SUBSCRIPTION = "premium_subscription";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (storeController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized()) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(Gem100, ProductType.Consumable);
        builder.AddProduct(Gem600, ProductType.Consumable);
        builder.AddProduct(Gem1550, ProductType.Consumable);
        builder.AddProduct(Gem4650, ProductType.Consumable);
        builder.AddProduct(Gem12250, ProductType.Consumable);

        //builder.AddProduct(PRODUCT_NO_ADS, ProductType.NonConsumable);
        //builder.AddProduct(PRODUCT_SUBSCRIPTION, ProductType.Subscription);

        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    public void BuyProduct(string productId)
    {
        if (IsInitialized())
        {
            Product product = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("Product not found or not available for purchase");
            }
        }
        else
        {
            Debug.Log("IAP not initialized");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("IAP not initialized");
            return;
        }
        var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
        apple.RestoreTransactions((success) =>
        {
            Debug.Log("Restore purchases completed: " + success);
        });
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == Gem100)
        {
            Debug.Log("Gem100");
            // Grant Gems to player
        }
        else if (args.purchasedProduct.definition.id == Gem600)
        {
            Debug.Log("Gem600");
           
        }
        else if (args.purchasedProduct.definition.id == Gem1550)
        {
            Debug.Log("Gem1550");

        }
        else if (args.purchasedProduct.definition.id == Gem4650)
        {
            Debug.Log("Gem4650");

        }
        else if (args.purchasedProduct.definition.id == Gem12250)
        {
            Debug.Log("Gem12250");

        }
        //else if (args.purchasedProduct.definition.id == PRODUCT_NO_ADS)
        //{
        //    Debug.Log("No Ads Purchased");
        //    PlayerPrefs.SetInt("NoAds", 1);
        //}
        //else if (args.purchasedProduct.definition.id == PRODUCT_SUBSCRIPTION)
        //{
        //    Debug.Log("Subscription Activated");
        //    PlayerPrefs.SetInt("Subscription", 1);
        //}
        else
        {
            Debug.Log("Purchase failed: Unknown product");
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("Purchase failed: " + failureReason);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
        Debug.Log("IAP Initialized Successfully");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("IAP Initialization Failed: " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        //throw new NotImplementedException();
    }
}
