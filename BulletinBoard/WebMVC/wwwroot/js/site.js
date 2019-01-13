//import "./css/main.css";

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//автозаполнение для поиска
$(document).ready(function () {
    $("#searchedText").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Home/Index1",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.title, value: item.title };
                    }))
                }
            })
        },
        messages: {
            noResults: "", results: ""
        }
    });
});

//загрузка списка категорий
$(document).ready(function () {
    var i = 0;
    
    var cat = [];
    console.log('launching AJAX request Category');    
    $.ajax({
        method: 'GET',
        url: '/home/GetCategories',
        success: function (data) {
            console.log('AJAX success, filling reg array');
            $.each(data, function () {
                cat[i] = {
                    "categoryId": this.categoryId,
                    "name": this.categoryName,
                    "subcategory": this.subcategories
                }
                i++;

                if (i === 1) {
                    $("#selectCategory").append($("<option style='font-weight: bold;'></option>").val(this.categoryId).text(this.categoryName));
                }
                else {
                    $("#selectCategory").append($("<option style='color: #333; background-color: #C0C0C0;'></option>").val(this.categoryId).text(this.categoryName));
                }
                
                $.each(this.subcategories, function (index1, name1) {
                    $("#selectCategory").append($("<option></option>").val(index1).text(name1));                    
                });

            });
            console.log('finishing filling reg array');
        }
    });
    console.log('returning reg array');
});

//загрузка списка регионов
$(document).ready(function () {
    var i = 0;
    var reg = [];

    console.log('launching AJAX request');

    $.ajax({
        method: 'GET',
        url: '/home/getregions',
        success: function (data) {
            console.log('AJAX success, filling reg array');
            $.each(data, function (index, name) {
                reg[i] = {
                    "id": index,
                    "name": name
                }
                i++;
                $("#selectRegion").append($("<option></option>").val(index).text(name));
            });
            console.log('finishing filling reg array');
        }
    });
    console.log('returning reg array');
});

       
$(function () {
    $("#sign-up-form-phone-input").mask("+7(999) 999-99-99");
});

    