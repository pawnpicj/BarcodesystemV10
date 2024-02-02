class IEventCopyFromPO {

    ChoosePO_Click(temporyListPO, TbCopyFromPO) {
        var docNum = "";
        var cardCode = "";
        var cardName = "";
        var remark = "";
        var doctotal = 0;
        var discountPercentage = 0;
        var discountAmount = 0;
        var discountLine = 0;
        var lineTotal = 0;
        var toBinLocation = "";
        var slpCode = "";
        var slpName = "";
        var LinesAR = [];
        var LineBatchSerial = [];
        for (var i = 0; i < temporyListPO.length; i++) {
            var data = TbCopyFromPO.row(temporyListPO[i]).data();
            docNum = data.docNum
            remark = remark + data.docNum + ". ";
            doctotal = doctotal + data.docTotal;
            discountPercentage = data.discPrcnt;
            discountAmount = data.discountAMT;
            cardCode = data.cardCode;
            cardName = data.cardName;
            toBinLocation = data.toBinLocation;
            slpCode = data.slpCode;
            slpName = data.slpName;
            for (var k = 0; k < data.line.length; k++) {
                var objLineItem = {};
                discountLine = discountLine + data.line[k].discountAMT;
                lineTotal = lineTotal + data.line[k].lineTotal;
                objLineItem.ItemCode = data.line[k].itemCode;
                objLineItem.UnitPrice = data.line[k].price;
                objLineItem.Discount = data.line[k].discPrcnt;
                objLineItem.DiscountAmount = data.line[k].discountAMT;
                objLineItem.Quantity = data.line[k].quatity;
                objLineItem.InputQuantity = data.line[k].inputQuantity;
                objLineItem.PriceBeforeDis = data.line[k].priceBeforeDis;
                objLineItem.LineTotal = data.line[k].lineTotal;
                objLineItem.UomName = data.line[k].uomName;
                objLineItem.TaxCode = data.line[k].taxCode;
                objLineItem.GrossPrice = data.line[k].priceAfVAT;
                objLineItem.Whs = data.line[k].whsCode;
                objLineItem.DocEntry = data.docEntry;
                objLineItem.LineNum = data.line[k].lineNum;
                objLineItem.ManageItem = data.line[k].manageItem;
                objLineItem.Patient = data.line[k].patient;
                objLineItem.TranferNo = data.line[k].tranferNo;
                objLineItem.YesNo = "No";
                objLineItem.Serial = [];
                objLineItem.Batches = [];
                LinesAR.push(objLineItem);
            }

        }
        let responseBack = {};
        responseBack.docNum = docNum;
        responseBack.cardCode = cardCode;
        responseBack.cardName = cardName;
        responseBack.remark = remark;
        responseBack.docTotal = doctotal;
        responseBack.discountPercentage = discountPercentage;
        responseBack.discountAmount = discountAmount;
        responseBack.discountLine = discountLine;
        responseBack.lineTotal = lineTotal;
        responseBack.toBinLocation = toBinLocation;
        responseBack.slpCode = slpCode;
        responseBack.slpName = slpName;
        responseBack.linesAR = LinesAR;
        //responseBack.lineBatchSerial = LineBatchSerial;
        return responseBack;
    }

    ChoosePONofity_Click(temporyListPO, TbCopyFromPO) {
        var docNum = "";
        var cardCode = "";
        var cardName = "";
        var remark = "";
        var doctotal = 0;
        var discountPercentage = 0;
        var discountAmount = 0;
        var discountLine = 0;
        var lineTotal = 0;
        var toBinLocation = "";
        var slpCode = "";
        var slpName = "";
        var sqRemark = "";
        var LinesAR = [];
        var LineBatchSerial = [];
        for (var i = 0; i < temporyListPO.length; i++) {
            var data = TbCopyFromPO.row(temporyListPO[i]).data();
            //console.log("IEventCopyFromPORow ->");
            //console.log(data);
            docNum = data.docNum
            remark = remark + data.docNum + ". ";
            doctotal = doctotal + data.docTotal;
            discountPercentage = data.discPrcnt;
            discountAmount = data.discountAMT;
            cardCode = data.cardCode;
            cardName = data.cardName;
            toBinLocation = data.toBinLocation;
            slpCode = data.slpCode;
            slpName = data.slpName;
            sqRemark = data.sqRemark;
            for (var k = 0; k < data.line.length; k++) {
                var objLineItem = {};
                discountLine = discountLine + data.line[k].discountAMT;
                lineTotal = lineTotal + data.line[k].lineTotal;
                objLineItem.ItemCode = data.line[k].itemCode;
                objLineItem.UnitPrice = data.line[k].price;
                objLineItem.Discount = data.line[k].discPrcnt;
                objLineItem.DiscountAmount = data.line[k].discountAMT;
                objLineItem.Quantity = data.line[k].quatity;
                objLineItem.InputQuantity = data.line[k].inputQuantity;
                objLineItem.PriceBeforeDis = data.line[k].priceBeforeDis;
                objLineItem.LineTotal = data.line[k].lineTotal;
                objLineItem.UomName = data.line[k].uomName;
                objLineItem.TaxCode = data.line[k].taxCode;
                objLineItem.GrossPrice = data.line[k].priceAfVAT;
                objLineItem.Whs = data.line[k].whsCode;
                objLineItem.DocEntry = data.docEntry;
                objLineItem.LineNum = data.line[k].lineNum;
                objLineItem.ManageItem = data.line[k].manageItem;
                objLineItem.Patient = data.line[k].patient;
                objLineItem.TranferNo = data.line[k].tranferNo;
                objLineItem.YesNo = "No";
                objLineItem.Serial = [];
                objLineItem.Batches = [];
                LinesAR.push(objLineItem);
            }

            for (var l = 0; l < data.lineBatchSerial.length; l++) {
                var objLineItemBatchSerial = {};
                objLineItemBatchSerial.lineNo = data.lineBatchSerial[l].lineNum;
                objLineItemBatchSerial.itemCode = data.lineBatchSerial[l].itemCode;
                objLineItemBatchSerial.itemName = data.lineBatchSerial[l].dscription;
                objLineItemBatchSerial.quatity = data.lineBatchSerial[l].quatity;
                objLineItemBatchSerial.inputQuantity = data.lineBatchSerial[l].inputQuantity;
                objLineItemBatchSerial.uomCode = data.lineBatchSerial[l].uomCode;
                objLineItemBatchSerial.price = data.lineBatchSerial[l].price;
                objLineItemBatchSerial.priceBeforeDis = data.lineBatchSerial[l].priceBeforeDis;
                objLineItemBatchSerial.grossPrice = data.lineBatchSerial[l].grossPrice;
                objLineItemBatchSerial.batchSerialType = data.lineBatchSerial[l].batchSerialType;
                objLineItemBatchSerial.batchNumber = data.lineBatchSerial[l].batchNumber;
                objLineItemBatchSerial.serialNumber = data.lineBatchSerial[l].serialNumber;
                objLineItemBatchSerial.warehouse = data.lineBatchSerial[l].warehouse;
                objLineItemBatchSerial.binEntry = data.lineBatchSerial[l].binEntry;
                objLineItemBatchSerial.binLocation = data.lineBatchSerial[l].binLocation;
                objLineItemBatchSerial.patient = data.lineBatchSerial[l].patient;
                objLineItemBatchSerial.lineSap = data.lineBatchSerial[l].baseLine;
                LineBatchSerial.push(objLineItemBatchSerial);
            }
        }
        let responseBack = {};
        responseBack.docNum = docNum;
        responseBack.cardCode = cardCode;
        responseBack.cardName = cardName;
        responseBack.remark = remark;
        responseBack.docTotal = doctotal;
        responseBack.discountPercentage = discountPercentage;
        responseBack.discountAmount = discountAmount;
        responseBack.discountLine = discountLine;
        responseBack.lineTotal = lineTotal;
        responseBack.toBinLocation = toBinLocation;
        responseBack.slpCode = slpCode;
        responseBack.slpName = slpName;
        responseBack.sqRemark = sqRemark;
        responseBack.linesAR = LinesAR;
        responseBack.lineBatchSerial = LineBatchSerial;
        return responseBack;
    }

}

