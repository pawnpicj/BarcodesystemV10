let array = [];

let EventSaveGoodReceiptPO = {
    iEventSaveGoodReceipt: new IEventSaveGoodReceipt(),
    validDateForm(array, condition) {
        var i = EventSaveGoodReceiptPO.iEventSaveGoodReceipt.validForm(array, condition);
        return i;
    },

    valiDateLine(array) {        
        console.log("Array valiDateLine :-")
        console.log(array);
        var i = EventSaveGoodReceiptPO.iEventSaveGoodReceipt.validLine(array);
        return i;
    },

    sendDelivery(url) {
        var validate = 1;
        validate = EventSaveGoodReceiptPO.validDateForm([
                { id: "CusID", value: "Please Enter Customer Code!" },
                { id: "SeriesID", value: "Please Select Series Code!" },
                { id: "DocDate", value: "Please Select DocDate !" },
                { id: "DocumentDate", value: "Please Select DocumentDate !" },
                { id: "BPDocCurr", value: "Please Select BP Currency!" }
            ],
            "");
        if (validate === 0) {
            //console.log("Line Here");
            //console.log(LinesAR);
            //Send to IEventSaveGoodReceipt.validLine
            validate = EventSaveGoodReceiptPO.valiDateLine(xLinesAR);
            //console.log(validate);
            if (validate === 0) {

                dLine = [];
                dSubLine = {};
                //console.log(LinesAR);     

                for (let x = 0; x < xLinesAR.length; x++) {
                    dSubLine = {};
                    //console.log("Id :-" + x);
                    dSubLine.DocEntry = xLinesAR[x].DocEntry;
                    dSubLine.LineNum = xLinesAR[x].LineNum;
                    dSubLine.ItemCode = xLinesAR[x].ItemCode;
                    dSubLine.Quantity = xLinesAR[x].Quantity;
                    dSubLine.PriceBeforeDis = xLinesAR[x].PriceBeforeDis;
                    dSubLine.Discount = xLinesAR[x].Discount;
                    dSubLine.TaxCode = xLinesAR[x].TaxCode;
                    dSubLine.PriceAfterVAT = xLinesAR[x].GrossPrice;
                    dSubLine.Whs = xLinesAR[x].Whs;
                    dSubLine.Patient = xLinesAR[x].Patient;
                    dSubLine.ManageItem = xLinesAR[x].ManageItem;
                    dSubLine.BinEntry = xLinesAR[x].BinEntry;
                    dSubLine.YesNo = xLinesAR[x].YesNo;

                    bLine = [];
                    bSubLine = {};
                    for (let b = 0; b < xLinesAR[x].Batches.length; b++) {
                        bSubLine = {};
                        bSubLine.ItemCode = xLinesAR[x].Batches[b].ItemCode;
                        bSubLine.qty = xLinesAR[x].Batches[b].qty;
                        bSubLine.BatchNumber = xLinesAR[x].Batches[b].BatchNumber;
                        bSubLine.BinEntry = xLinesAR[x].Batches[b].BinEntry;
                        bLine.push(bSubLine);
                    }

                    sLine = [];
                    sSubLine = {};
                    for (let s = 0; s < xLinesAR[x].Serial.length; s++) {
                        sSubLine = {};
                        sSubLine.ItemCode = xLinesAR[x].Serial[s].ItemCode;
                        sSubLine.qty = xLinesAR[x].Serial[s].qty;
                        sSubLine.SerialNumber = xLinesAR[x].Serial[s].SerialNumber;
                        sSubLine.BinEntry = xLinesAR[x].Serial[s].BinEntry;
                        sLine.push(sSubLine);
                    }

                    dSubLine.Batches = bLine;
                    dSubLine.Serial = sLine;
                    dLine.push(dSubLine);
                }
                console.log(dLine);

                var sendGoodReceiptPO = {};
                sendGoodReceiptPO.CardCode = $("#CusID").val();
                sendGoodReceiptPO.Series = $("#SeriesID").val();
                sendGoodReceiptPO.DocDate = $("#DocDate").val();
                sendGoodReceiptPO.TaxDate = $("#DocumentDate").val();
                sendGoodReceiptPO.OrderNumber = $("#OrderNumberID").val();
                sendGoodReceiptPO.CurrencyCode = $("#BPDocCurr").val();
                sendGoodReceiptPO.DiscountPercent = $("#DisPer").val();
                sendGoodReceiptPO.SlpCode = $("#txtSlpCode").val();
                sendGoodReceiptPO.DocTotal = $("#Total").val();
                sendGoodReceiptPO.Remark = $("#Remark").val();
                sendGoodReceiptPO.Sq_Remark = $("#SQ_Remark").val();
                sendGoodReceiptPO.Lines = dLine;
                console.log("Data for SAP ->");
                console.log(sendGoodReceiptPO);
                $.ajax({
                    url: url,
                    type: "POST",
                    dataType: "JSON",
                    data: { sendDelivery: sendGoodReceiptPO },
                    success: function(data) {
                        console.log(data);
                        alert("Successfull");
                        document.getElementById("frmLoading").style.display = "none";
                        location.reload();
                        //$("#SerialNumber").val(data[0].serialAndBatch);
                        //$("#txtScriptID").val(data[0].script);
                    },
                    error: function (erro) {
                        console.log(erro.responseText);
                        alert("Internal Error -> " + erro.responseText);
                        document.getElementById("frmLoading").style.display = "none";
                    }
                });
            } else {
                document.getElementById("frmLoading").style.display = "none";
            }
        } else {
            document.getElementById("frmLoading").style.display = "none";
        }
    },
    LineDiscountPer() {
        EventSaveGoodReceiptPO.iEventSaveGoodReceipt.LineDiscountPer();
    },
    LinDiscountAMT() {
        EventSaveGoodReceiptPO.iEventSaveGoodReceipt.LinDiscountAMT();
    },
}