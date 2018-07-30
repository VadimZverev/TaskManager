$(document).ready(function () {

    $(document).on('click', '.ico-edit', function () {
        var _id = $(this).data('id');
		window.location.host;
        window.location.href = '/Accaunt/EditUserData/' + _id;
		
        console.log(window.location.href);
    });

    $(document).on('click', '.ico-back', function () {
        var _id = $(this).data('id');
        window.location.host;
        window.location.href = '/Home/Index/';

        console.log(window.location.href);
    });

    $(document).on('click', '.ico-info', function () {
        var _id = $(this).data('id');
        window.location.host;
        window.location.href = '/Accaunt/UserDataDetails/' + _id;

        console.log(window.location.href);
    });

    $(document).on('click', '.ico-delete', function (e) {
        var _id = $(this).data('id');
        var el_tr = $(this).closest("tr");
        var el_tbody = $(el_tr).closest('tbody');

        if (confirm("Вы действительно хотите удалить?")) {

            $.ajax({
                type: "POST",
                url: "/Accaunt/DeleteUser/",
                // передача в качестве объекта
                // поля будут закодированые через encodeURIComponent автоматически
                data: { id: _id }
                ,
                success: function (data) {

                    if (data.result == true) {
                        el_tbody.find(el_tr).remove();
                    }
                    else {
                        alert("Данная запись не найдена.");
                    }
                   
                    console.log(data);
                    console.log(data.msg);
                    
                },
                error: function (data) {
                    alert('Нет ответа от сервера.');
                }
            });
        }
    });
});
