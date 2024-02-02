
let GetMasterData = {
    overallFunction: new GetMasterGoodReceiptPO(),
    getSeries(DocDate, currentDate, url, ObjectCode) {
        GetMasterData.overallFunction.getSeries(DocDate, currentDate, url, ObjectCode);
    },
    getSaleEmployee(url) {
        console.log("SaleEmployee");
        GetMasterData.overallFunction.getSaleEmployee(url);
    },
    getCurrency(url, cardCode) {
        console.log("Currency");
        GetMasterData.overallFunction.getCurrency(url, cardCode);
    },
    getCustomer(url, cusType) {
        console.log("Customer");
        GetMasterData.overallFunction.getCustomer(url,cusType);
    },
    getPurchaseOrder(url, cardCode, typeShow) {
        console.log("PurchaseOrder");
        GetMasterData.overallFunction.getPurchaseOrder(url, cardCode, typeShow);
    },
    getGoodReceiptPO(url, cardCode) {
        console.log("GoodReceiptPO");
        GetMasterData.overallFunction.getGoodReceiptPO(url, cardCode);
    },
    getTaxCode(url) {
        GetMasterData.overallFunction.getTaxCode(url);
    },
    getWarehouse(url) {
        GetMasterData.overallFunction.getWarehouse(url);
    },
    getUomCode(url, ItemCode, UOMType) {
        GetMasterData.overallFunction.getUomCode(url, ItemCode, UOMType);
    }
}

let DataTableInit = {
    TableItemLine() {
        $('#TbAR').dataTable({
            bLengthChange: false,
            bFilter: false,
            bInfo: false,
            bPaginate: false,
            bSelect: true,
            ordering: false,
            data: LinesAR,
            columns:
                [
                    {
                        render: function (data, type, full, meta) {
                            if (full.ManageItem === "N") {
                                return "";
                            }
                            return '<button class="btn-sm btn-info" style="margin-left: 40%;" onClick="Btn_ClickBatchSerail(\'' + meta.row + '\',tbItemLine)"><i class="fas fa-barcode"></i></button>';
                        }
                    },
                    { data: "ItemCode", autoWidth: true },
                    {
                        data: "UnitPrice", autoWidth: true
                        //render: function (data, type, full, meta) { return '<input type="number" class="clsinput" style="padding:0px; position:absolute;width:100px;border:none;" onchange="PriceChange(' + meta.row + ')" id="tbPrice' + meta.row + '" value="' + full.UnitPrice + '">'; }, autoWidth: true
                    },
                    {
                        data: "Discount", autoWidth: true
                        //render: function (data, type, full, meta) { return '<input type="number" class="clsinput" style="padding:0px; position:absolute;width:100px;border:none;" onchange="DisChange('+meta.row+')" value="'+full.Discount+'" id="tbDis'+meta.row+'" >'; }, autoWidth: true
                    },
                    {
                        data: "DiscountAmount", autoWidth: true
                        //render: function (data, type, full, meta) { return '<input type="number" class="clsinput" style="padding:0px; position:absolute;width:100px;border:none;" onchange="DisamtChange('+meta.row+')" value="'+full.DiscountAmount+'" id="tbDisamt'+meta.row+'" >'; }, autoWidth: true
                    },
                    {
                        data: "Quantity", autoWidth: true
                        //render: function (data, type, full, meta) { return '<input type="number" class="clsinput" style="padding:0px; position:absolute;width:40px;border:none;" onchange="QtyChange(' + meta.row + ')" id="tbQty' + meta.row + '" value="' + full.Quantity + '" readonly=true; >'; }, autoWidth: true
                    },
                    { data: "InputQuantity", autoWidth: true },                    
                    { data: "PriceBeforeDis", autoWidth: true },
                    { data: "LineTotal", autoWidth: true },
                    { data: "UomName", autoWidth: true },
                    { data: "TaxCode", autoWidth: true },
                    { data: "GrossPrice", autoWidth: true },
                    { data: "Whs", autoWidth: true },
                    { data: "Patient", autoWidth: true },
                    { data: "TranferNo", autoWidth: true },
                    {
                        data: "LineNum", autoWidth: true
                        //render: function (data, type, full, meta) { return meta.row; }
                    },
                    {
                        data: "InputQuantity",
                        render: function (data, type, full, meta) {
                            return '<input type="text" id="txtBalance' + meta.row + '" value="' + data + '" size="2" class="styBalance"/>&nbsp;<input type="text" id="txtBalanceX' + meta.row + '" value="' + data +'" size="2" readonly/>';
                        }
                    }
                ],
            rowCallback: function (row, data, index) {
                //$('td', row).css('background-color', '#ffffff');
                //$('td', row).css('color', '#717171');
            }
        });
    },

    TableListBatchSerial() {
        $('#TbLineBatchSerial').dataTable({
            bLengthChange: false,
            bFilter: false,
            bInfo: false,
            bPaginate: false,
            bSelect: true,
            ordering: false,
            data: LineBatchSerial,
            columns:
                [                    
                    { data: "lineNo", autoWidth: true },
                    { data: "itemCode", autoWidth: true},
                    { data: "itemName", autoWidth: true},
                    { data: "quatity", autoWidth: true},
                    { data: "inputQuantity", autoWidth: true },
                    { data: "uomCode", autoWidth: true },
                    { data: "grossPrice", autoWidth: true },
                    { data: "batchSerialType", autoWidth: true },
                    { data: "batchNumber", autoWidth: true },
                    { data: "serialNumber", autoWidth: true },
                    { data: "warehouse", autoWidth: true },
                    { data: "binEntry", autoWidth: true },
                    { data: "binLocation", autoWidth: true },
                    { data: "patient", autoWidth: true },
                    { data: "lineSap", autoWidth: true }
                ],
            rowCallback: function (row, data, index) {
            }
        });
    },

    TableSearchItemCode() {
        $('#TbItem').DataTable({
            responsive: true,
            bLengthChange: false,
            binfo: false,
            data: LItm,
            columns: [
                { data: "itemCode", autoWidth: true },
                { data: "itemName", autoWidth: true },
                { data: "unitPrice", autoWidth: true },
                { data: "quantity", autoWidth: true },
                { data: "barCode", autoWidth: true }
            ],
            rowCallback: function (row, data, index) {
                //$('td', row).css('background-color', '#ffffff');
                //$('td', row).css('color', '#717171');
            }
        });
    },
    TablePurchaseOrder() {
        $('#TbCopyFromPO').DataTable({
            responsive: true,
            ordering: false,
            bLengthChange: false,
            binfo: false,
            data: LCopyFromPO,
            columns: [
                { data: "docNum", autoWidth: true },
                { data: "cardName", autoWidth: true },
                { data: "docDate", autoWidth: true },
                { data: "docTotal", autoWidth: true },
                { data: "docStatus", autoWidth: true }
            ],
            rowCallback: function (row, data, index) {
            }
        });
    },
    TableGoodReturn() {
        $('#TbCopyToGoodReturn').DataTable({
            responsive: true,
            bLengthChange: false,
            binfo: false,
            data: LCopyToGoodReturn,
            columns: [
                { data: "docNum", autoWidth: true },
                { data: "cardName", autoWidth: true },
                { data: "docDate", autoWidth: true },
                { data: "docTotal", autoWidth: true },
                { data: "docStatus", autoWidth: true }
            ],
            rowCallback: function (row, data, index) {
            }
        });
    },
    TableCustomer() {
        $('#TbCusCode').DataTable({
            responsive: true,
            bLengthChange: false,
            binfo: false,
            data: LCus,
            columns: [
                { data: "cardCode", autoWidth: true },
                { data: "cardName", autoWidth: true },
                { data: "phone", autoWidth: true },
                { data: "address", autoWidth: true }
            ],
            rowCallback: function (row, data, index) {
                //$('td', row).css('background-color', '#ffffff');
                //$('td', row).css('color', '#717171');
            }
        });
    },
    TableSerialGen() {
        $('#TbSerial').append('<caption style="caption-side: top">Select</caption>');
        $('#TbSerial').dataTable({
            bLengthChange: false,
            bFilter: true,
            bInfo: false,
            bPaginate: true,
            data: lsSerial,
            columns:
                [
                    {
                        render: function (data, type, full, meta) { return '<input type="checkbox" class="clsinput" style="padding:0px; position:absolute;width:100px;border:none;" onchange="AddRowSerial(' + meta.row + ')" id="chkSerialSelect' + meta.row + '">'; },
                        autoWidth: true
                    },
                    {
                        data: "serialNumber",
                        autoWidth: true
                        // render: function (data, type, full, meta) { return '<input type="number" class="clsinput" style="padding:0px; position:absolute;width:100px;border:none;" onchange="PriceChange(' + meta.row + ')" id="tbPrice' + meta.row + '" value="' + full.UnitPrice + '">'; }, autoWidth: true
                    },
                    {
                        data: "qty",
                        autoWidth: true
                        //render: function (data, type, full, meta) { return '<input type="number" class="clsinput" style="padding:0px; position:absolute;width:100px;border:none;" onchange="DisChange('+meta.row+')" value="'+full.Discount+'" id="tbDis'+meta.row+'" >'; }, autoWidth: true
                    }
                    //,{
                    //    render: function (data, type, full, meta) {
                    //        return '<button class="btn-sm btn-danger" style="margin-left: 40%;" onClick="EventItemAdd.DeleteArray(lsSerial,\'' + full.SerialNumber + '\')"><i class="fas fa-trash-alt"></i></button>';
                    //    }
                    //}
                ],
            rowCallback: function (row, data, index) {
                //$('td', row).css('background-color', '#ffffff');
                //$('td', row).css('color', '#717171');
            }
        });
    },
    TableBatch() {
        $('#TbBatch').append('<caption style="caption-side: top">Selected</caption>');
        $('#TbBatch').DataTable({
            responsive: true,
            bLengthChange: false,
            binfo: false,
            data: LBatch,
            columns: [
                {
                    render: function (data, type, full, meta) { return '<input type="checkbox" class="clsinput" style="margin:auto; width:100%;" onchange="AddRowBatch(' + meta.row + ')" id="chkBatchSelect' + meta.row + '">'; },
                    autoWidth: true
                },
                { data: "batchNumber", autoWidth: true },
                {
                    render: function (data, type, full, meta) { return '<input type="number" class="clsinput" style="padding:0px; position:absolute;width:75px;border-color: black;" onchange="OnInputBatchChange(' + meta.row + ')" id="txtInputBatch' + meta.row + '">'; },
                    autoWidth: true
                },
                { data: "qty", autoWidth: true },
                { data: "expDate", autoWidth: true }
            ],
            rowCallback: function (row, data, index) {
                //$('td', row).css('background-color', '#ffffff');
                //$('td', row).css('color', '#717171');
            }
        });
    },
    TabletmpBatchFromTo() {
        $('#tmpBatchFromTo').append('<caption style="caption-side: top">Select</caption>');
        $('#tmpBatchFromTo').DataTable({
            responsive: true,
            bLengthChange: false,
            binfo: false,
            data: LtmpBatch,
            columns: [
                {
                    render: function (data, type, full, meta) { return '<input type="checkbox" class="clsinput" style="margin:auto; width:100%;" onchange="AddRowRemoveBatch(' + meta.row + ')" id="chkBatchUnSelect' + meta.row + '">'; },
                    autoWidth: true
                },
                { data: "batchNumber", autoWidth: true },
                { data: "qty", autoWidth: true },
                { data: "expDate", autoWidth: true },
            ],
            rowCallback: function (row, data, index) {
                //$('td', row).css('background-color', '#ffffff');
                //$('td', row).css('color', '#717171');
            }
        });
    },
    TableSerialFromTo() {
        $('#tmpSerial').append('<caption style="caption-side: top">Selected</caption>');
        $('#tmpSerial').DataTable({
            responsive: true,
            bLengthChange: false,
            binfo: false,
            data: LtmpSerial,
            columns: [
                {
                    render: function (data, type, full, meta) { return '<input type="checkbox" class="clsinput" style="margin:auto; width:100%;" onchange="AddRowRemoveSerial(' + meta.row + ')" id="chkRemoveSerial' + meta.row + '">'; },
                    autoWidth: true
                },
                { data: "serialNumber", autoWidth: true },
                { data: "qty", autoWidth: true }
            ],
            rowCallback: function (row, data, index) {
                //$('td', row).css('background-color', '#ffffff');
                //$('td', row).css('color', '#717171');
            }
        });
    },
    TableBarCode() {
        $('#TbBarCode').DataTable({
            responsive: true,
            bLengthChange: false,
            binfo: false,
            data: LBarCode,
            columns: [
                { data: "barCode", autoWidth: true },
                { data: "barCodeName", autoWidth: true },
                { data: "itemCode", autoWidth: true },
                { data: "itemName", autoWidth: true },
                { data: "uomCode", autoWidth: true },
                { data: "price", autoWidth: true },
                { data: "uomName", autoWidth: true, visible: false },
                { data: "manageItem", autoWidth: true, visible: false }
            ],
            rowCallback: function (row, data, index) {
                //$('td', row).css('background-color', '#ffffff');
                //$('td', row).css('color', '#717171');
            }
        });
    }
}

let DataMethod ={
    method: new DataTableMethod(), 
    AddClassSelected(id,table)
    {
        DataMethod.method.AddSelectClass(id,table);
    },
    arrayRemove(arr, value) {
        let arr1 = arr.filter(function (ele) {
            return ele != value;
        });
        return arr1;
    }
}

