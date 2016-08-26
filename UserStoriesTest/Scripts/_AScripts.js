//function updateCart(idsend, method) {

//    console.log('updateCart');
//    $.ajax({
//        type: 'GET',
//        cache: false,
//        data: { methode: method, id: idsend },
//        url: '/Products/UpdateCart',
//        success: function (result) {
//            console.log('done');
//            $("#checkout").html(result);

//        }
//    });
//}

function addToCart(idsent) {
    console.log('addtocart : ' + idsent);
    $.ajax({
        type: 'GET',
        cache : false,
        data: { methode: 'add', id: idsent },
        url: 'Products/ProductsCount',
        //error: function (jqXHR,textStatus, errorThrown) {
        //    console.log('jqXHR : '+errorThrown);
        //    console.log('testStatus : ' + textStatus)
        //    console.log('errorThrown : ' + errorThrown)
        //},
        error: function (error) {
            console.log('error : ' + error)
        },
        success: function (result) {
            console.log('done');
            $("#checkout").html(result);
           
        }
    });
}

function removeFromCart(idsent) {
    console.log('removefromcart : ' + idsent);
    $.ajax({
        type: 'GET',
        cache : false,
        data: { methode: 'remove', id: idsent },
        url: 'Products/ProductsCount',
        error: function (jqXHR, textStatus, errorThrown) {
            console.log('jqXHR : ' + errorThrown);
            console.log('testStatus : ' + textStatus)
            console.log('errorThrown : ' + errorThrown)
        },
        success: function (result) {
            console.log('done');
            $("#checkout").html(result);
           
        }
    });
}

function paymentcapture(payid) {
    
    $.ajax({
        data: { reference: payid },
        url: 'PostSale/DirectCapture'
    })
}
function directquery(payid) {
    console.log('on y va...');
    $.ajax({
        data: { payid: payid },
        url: 'PostSale/DirectQuery'
    })
}
function generateBatch() {
    //console.log('tralala');
    var references = new Array();
    $('#paymentstable input[type = checkbox]').each(function () {
        if (this.checked) {
            var value = (this.value);
            //console.log(value);
            references.push(value);
        }
    })
    console.log(references);
    $.ajax({
        traditional: true,
        //dataType: "json",
        data: { references: references },
        url: 'PostSale/GenerateBatch'
    }).done(function (partialviewresult) {
        console.log('oui');
        //console.log(partialviewresult)
        $("#batchform").html(partialviewresult);
    });
};