﻿let EventSaveGoodReceiptPO = {
    iEventSaveGoodReceipt: new IEventSaveGoodReceipt(),
    validDateForm(array, condition) {
        var i = EventSaveGoodReceiptPO.iEventSaveGoodReceipt.validForm(array, condition);
        return i;
    },
    valiDateLine(array) {
        var i = EventSaveGoodReceiptPO.iEventSaveGoodReceipt.validLine(array);
        return i;
    },
    sendGoodReceiptPO() {
        var validate = 1;
        validate = EventSaveGoodReceiptPO.validDateForm([
                { id: "CusID", value: "Please Enter Customer Code!" },
                { id: "SeriesID", value: "Please Select Series Code!" },
                { id: "DocDate", value: "Please Select DocDate !" },
                { id: "DocumentDate", value: "Please Select DocumentDate !" },
                { id: "BPDocCurr", value: "Please Select BP Currency!" }
            ],
            "");
        validate = EventSaveGoodReceiptPO.valiDateLine(LinesAR);
        if (validate === 0) {
            var sendGoodReceiptPO = {};
            sendGoodReceiptPO.CardCode = $("#CusID").val();
            sendGoodReceiptPO.Series = $("#SeriesID").val();
            sendGoodReceiptPO.DocDate = $("#DocDate").val();
            sendGoodReceiptPO.TaxDate = $("#DocumentDate").val();
            sendGoodReceiptPO.OrderNumber = $("#OrderNumberID").val();
            sendGoodReceiptPO.CurrencyCode = $("#BPDocCurr").val();
            sendGoodReceiptPO.SlpCode = $("#SaleEmp").val();
            sendGoodReceiptPO.Remark = $("#Remark").val();
            sendGoodReceiptPO.Line = LinesAR;
            console.log(sendGoodReceiptPO);
            alert("Successfull");
            $.ajax({
                url: '@Url.Action("CreateGoodsReceiptPoResult", "GoodsReceiptPO")',
                type: "POST",
                dataType: "JSON",
                data: { goodReceiptPo: sendGoodReceiptPO },
                success: function (data) {
                    console.log(data);
                    //$("#SerialNumber").val(data[0].serialAndBatch);
                    //$("#txtScriptID").val(data[0].script);
                },
                error: function (erro) {
                    alert(erro.message);
                }
            });
        }
    }
}