class GetMasterGoodReceiptPO {
    getSeries(dateOfMonth, currentDate, url, objectCode) {
        if (dateOfMonth == "") {
            dateOfMonth = formatDate(currentDate);
        }
        $.ajax({
            url: url,
            type: "GET",
            data: { objectCode: objectCode, dateOfMonth: dateOfMonth },
            dataType: "JSON",
            success: function(data) {
                $("#SeriesID").empty();
                for (var i = 0; i < data.length; i++) {
                    $("#SeriesID").append('<option value="' + data[i].code + '">' + data[i].name + "</option>");
                }
            },
            error: function(erro) {
                console.log(erro.responseText);
            }
        });
    }

    getSaleEmployee(url) {
        $.ajax({
            url: url,
            type: "GET",
            dataType: "JSON",
            success: function(data) {
                $("#SaleEmp").empty();
                for (var i = 0; i < data.length; i++) {
                    $("#SaleEmp").append('<option value="' + data[i].code + '">' + data[i].name + "</option>");
                }
            },
            error: function(erro) {
                console.log(erro.responseText);
            }
        });
    }

    getCurrency(url, cardCode) {
        $.ajax({
            url: url,
            type: "GET",
            dataType: "JSON",
            data: { cardCode: cardCode },
            success: function(data) {
                $("#BPDocCurr").empty();
                for (var i = 0; i < data.length; i++) {
                    $("#BPDocCurr").append("<option value='" + data[i].code + "'>" + data[i].name + "</option>");
                }
                $("#DocCurr").val($("#BPDocCurr").val());
            }
        });
    }

    getCustomer(url, cusType) {
        $.ajax({
            url: url,
            type: "GET",
            data: { cusType: cusType },
            dataType: "JSON",
            success: function(data) {
                LCus = data;
                tbCus.clear();
                tbCus.rows.add(LCus);
                tbCus.search("").columns().search("").draw();
                $("input[aria-controls='TbCusCode']").focus();
            },
            error: function(erro) {
                console.log(erro.responseText);
            }
        });
    }

    getPurchaseOrder(url, cardCode, typeShow) {
        console.log("CardCode : " + cardCode);
        console.log("TypeShow : " + typeShow);
        $.ajax({
            url: url,
            type: "GET",
            data: { cardCode: cardCode, typeShow: typeShow },
            dataType: "JSON",
            success: function(data) {
                LCopyFromPO = data;
                //console.log("getPurchaseOrder");
                console.log(LCopyFromPO);
                TbCopyFromPO.clear();
                TbCopyFromPO.rows.add(LCopyFromPO);
                TbCopyFromPO.search('').draw();
            },
            error: function(erro) {
                console.log(erro.responseText);
            }
        });
    }

    getGoodReceiptPO(url, cardCode) {
        $.ajax({
            url: url,
            type: "GET",
            data: { CardCode: cardCode },
            dataType: "JSON",
            success: function (data) {
                console.log(data);
                LCopyToGoodReturn = data;
                console.log(LCopyToGoodReturn);
                TbCopyFromGoodReciptPO.clear();
                TbCopyFromGoodReciptPO.rows.add(LCopyToGoodReturn);
                TbCopyFromGoodReciptPO.search("").columns().search("").draw();
            },
            error: function (erro) {
                console.log(erro.responseText);
            }
        });
    }

    getTaxCode(url) {
        $.ajax({
            url: url,
            type: "GET",
            dataType: "JSON",
            success: function (data) {
                $("#TaxCode").empty();
                for (var i = 0; i < data.length; i++) {
                    $("#TaxCode").append("<option value='" + data[i].code + "'>" + data[i].name + "</option>");
                }
            }
        });
    }
    getWarehouse(url) {
        $.ajax({
            url: url,
            type: "GET",
            dataType: "JSON",
            success: function (data) {
                $("#whscode").empty();
                for (var i = 0; i < data.length; i++) {
                    $("#whscode").append("<option value='" + data[i].code + "'>" + data[i].name + "</option>");
                }
            }
        });
    }
    getUomCode(url, ItemCode, UOMType) {
        $.ajax({
            url: url,
            type: "GET",
            dataType: "JSON",
            data: { ItemCode: ItemCode, UOMType: UOMType },
            success: function (data) {
                $("#UomID").empty();
                for (var i = 0; i < data.length; i++) {
                    $("#UomID").append("<option value='" + data[i].code + "'>" + data[i].code +"-"+ data[i].name + "</option>");
                }
            }
        });
    }
}