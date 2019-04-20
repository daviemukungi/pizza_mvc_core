jq(function() {
    jq('.modal').modal();
    jq('select').material_select();

    jq('#order-table').DataTable({
        "order": [[0, 'desc']],
        "displayLength": 25,
    });

    jq('#Pizza_Id').change(function(){
        jq('#Pizza_Price').val(jq(this).val());
        UpdateToppingsPrices();
    }).change();

    jq('a.btn-add').click(function(){
        var pizza = jq('#Pizza_Id :selected').text();
        var topup = '';
        var count = 0;
        var amts = parseFloat(jq('#Pizza_Price').val());
        var json = '{pizza:"' + pizza + '", amount:'+amts+',basic:[';

        var basic = $("ul.basic-toppings li.collection-item");
        basic.each(function(idx, li) {
            if($(li).find('input')[0].checked){
                var name = $(li).data('name');
                var amnt = parseFloat($(li).find('span.ultra-small').html());

                amts += amnt;
                topup += ', ' + name;
                count++;

                json += '{id:' + $(li).data('idnt') + ', amount:' + amnt + '},';
            }
        });

        json += '],deluxe:[';

        var deluxe = $("ul.deluxe-toppings li.collection-item");
        deluxe.each(function(idx, li) {
            if($(li).find('input')[0].checked){
                var name = $(li).data('name');
                var amnt = parseFloat($(li).find('span.ultra-small').html());

                amts += amnt;
                topup += ', ' + name;
                count++;

                json += '{id:' + $(li).data('idnt') + ', amount:' + amnt + '},';
            }
        });
        
        json += ']}';

        if (count == 0) {
            topup = 'No Toppings'
        } 
        else if (count == 1) {
            topup = '1 Topping - ' + topup.substring(2, 100);
        }
        else {
            topup = count + ' Toppings - ' + topup.substring(2, 100);
        }

        var rows = '<tr><td>1 <input type="hidden" value="" />' + pizza + '</td><td>' + topup + '</td><td>' + amts + '</td></tr>';
        jq('#tbl-new-order tbody').append(rows);

        jq('#tbl-new-order tbody tr:last-child td').find('input').val(json);

        amts = 0.0;
        jq('#tbl-new-order tbody tr').each(function(idx, tr) {
            amts += parseFloat($(tr).find('td:last-child').text());
        });

        jq('#tbl-new-order tfoot tr th:last-child').html(amts);
    });

    jq('a.btn-save').click(function(){
        if ($("#tbl-new-order > tbody > tr").length == 0){
            Materialize.toast('<span>No Items Added for Submit</span><a class="btn-flat yellow-text right" href="#!">Close<a>', 3000);
            return;
        }

        var amts = parseFloat(jq('#tbl-new-order tfoot tr th:last-child').html());
        var json = '{status:1, amount: ' + amts + ', items:[';

        jq('#tbl-new-order tbody tr').each(function(idx, tr) {
            json += $(tr).find('td:first-child input').val() + ',';
        });

        json += ']}';

        console.log(json);

        jq('#Json').val(json);
        jq('form').submit();
    });

    jq('<i class="material-icons medium blue-text right modal-trigger" data-target="add-order-modal">add_shopping_cart</i>').insertBefore(jq("#order-table_filter label"));
});

function UpdateToppingsPrices(){
    var basic = $("ul.basic-toppings li.collection-item");
    var text = jq('#Pizza_Id :selected').text().toLowerCase();

    basic.each(function(idx, li) {
        var cost = $(li).data(text);
        $(li).find('span.ultra-small').html(cost);
    });

    var deluxe = $("ul.deluxe-toppings li.collection-item");
    deluxe.each(function(idx, li) {
        var cost = $(li).data(text);
        $(li).find('span.ultra-small').html(cost);
    });
}

