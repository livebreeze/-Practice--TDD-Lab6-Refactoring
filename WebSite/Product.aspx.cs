﻿using LogisticLib;
using System;

/** 重構之路* */
/* 1.使用 Selelium IDE, 在 CalculateFeeTest.cs 建立 UnitTest -> run test: green! */
/* 2.建立 ViewModel 類別, 增加 .aspx.cs & .aspx 之間的彈性 -> run test: green! Complexity:14 */
/* 3.抽出計算邏輯(private metohd Ctrl+R, Ctrl+M), 降低複雜度  -> run test: green! Complexity:6 */
/* 4.方法改成建立各運費類別 (名詞為類別名稱, 動詞為方法名稱), 建立 LogisticLib 邏輯層專案 -> run test: green! Complexity:6 */
/* 5.再抽出共用方法的時候就應該要做 unit test, Timely, 及時跟上 Production code, 補上! -> run all test: green! */
/* 6.抽出介面, 相同的事情就可以抽出判斷, 下一步便能做 DI, 達到切開主程式與運算邏輯的相依 */
/* 7.抽出 btn 事件中, (1)要顯示的運貨商名稱, (2)選擇要實作的運貨商 -> run all test: green! Complexity:3 */

public partial class Product : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        var product = this.GetShippingProduct();
        if (this.IsValid)
        {
            IShipping shipping = GetShipper(this.drpCompany.SelectedValue);
            this.lblCompany.Text = shipping.Name;

            if (shipping == null)
            {
                var js = "alert('發生不預期錯誤，請洽系統管理者');location.href='http://tw.yahoo.com/';";
                this.ClientScript.RegisterStartupScript(this.GetType(), "back", js, true);
            }

            shipping.CalculateFee(product);
            this.lblCharge.Text = product.ShippingFee.ToString();
        }
    }

    private IShipping GetShipper(string shipperId)
    {
        IShipping result = null;
        switch (shipperId)
        {
            case "1":
                result = new Blackcat();
                break;

            case "2":
                result = new HsinChu();
                break;

            case "3":
                result = new Postoffice();
                break;

            default:
                break;
        }

        return result;
    }

    private ShippingProduct GetShippingProduct()
    {
        var result = new ShippingProduct
        {
            Name = this.txtProductName.Text,
            Weight = Convert.ToDouble(this.txtProductWeight.Text),
            Size = new Size
            {
                Length = Convert.ToDouble(this.txtProductLength.Text),
                Width = Convert.ToDouble(this.txtProductWidth.Text),
                Height = Convert.ToDouble(this.txtProductHeight.Text)
            },
            Shipper = Convert.ToInt32(this.drpCompany.SelectedValue)
        };

        return result;
    }
}