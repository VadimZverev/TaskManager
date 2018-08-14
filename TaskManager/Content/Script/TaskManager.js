$(document).ready(function () {

    // возврат, пока не используется.
    $(document).on('click', '.ico-back', function () {
        var _id = $(this).data('id');
        window.location.host;
        window.location.href = '/Home/Index/';

        console.log(window.location.href);
    });

    // поиск
    $(document).on('click', '#search', function (e) {
        e.preventDefault();

        var name = $('#name').val();

        $("#loading").show();

        $.ajax({
            type: "POST",
            url: "/Home/ProjectSearch/",
            data: { name: name },
            success: function (data) {
                $('#results').html(data);
                $("#loading").hide();
            }
        });
    });

    /* _____Админ уровень. Работа с пользователями_____ */

    // Список пользователей.
    $(document).on('click', '#listUser', function (e) {
        e.preventDefault();

        $("#loading").show();

        window.location.host;
        window.location.href = '/Account/ListUser/';
    });

    // Подробности пользователя.
    $(document).on('click', '#userDataDetails', function (e) {
        e.preventDefault();

        var container = this.closest('td');
        var _id = $(container).find('#idUser').data('id');

        $("#loading").show();

        var url = "/Account/UserDataDetails/" + _id;
        $('#myModalBodyDiv1').load(url, function () {
            $("#loading").hide();
            $('#myModal1').modal('show');
            $('.modal-title').html('Информация о пользователе');

            $('.close').on('click', function () {
                $('.modal').modal('hide');
                $('.modal-title').empty();
                $('#myModalBodyDiv1').empty();
            });
        });
    });

    //Создание нового пользователя
    $(document).on('click', '#сreateUser', function (e) {
        e.preventDefault();

        $("#loading").show();

        var url = "/Account/CreateUser/";

        $('#myModalBodyDiv1').load(url, function () {

            $("#loading").hide();
            $('.modal-title').html('Новый пользователь');
            $('#myModal1').modal('show');

            var $form = $('#myForm');
            $.validator.unobtrusive.parse($form);

            $('.close').on('click', function () {
                $('.modal').modal('hide');
                $('.modal-title').empty();
                $('#myModalBodyDiv1').empty();
            });

            $form.submit(function () {

                if ($form.valid()) {
                    $.ajax({
                        type: "POST",
                        url: "/Account/CreateUser/",
                        data: $(this).serialize(),
                        success: function (data) {

                            if (data.result === true) {
                                $('.modal').modal('hide');
                                $(document).find('tbody').append(
                                    '<tr><td>' + data.fio +
                                    '</td><td>' + data.role +
                                    '</td>' +
                                    '<td><span id="idUser" hidden = "hidden"  data-id=' + data.id + '></span >' +
                                    '<span title="Подробнее" id="userDataDetails" class="ico-details icon-button">' +
                                    '</span><span title="Редактировать" id="editUserData" class="ico-edit icon-button">' +
                                    '</span><span title="Удалить пользователя" id="deleteUser" class="ico-delete icon-button">' +
                                    '</span ></td >'
                                );

                                console.log(data);

                                $('#myModalBodyDiv1').empty();
                            }
                            else if (data.result === false) {
                                $('.validation-summary-errors').removeAttr('hidden').find('li').html(data.msg);
                                //alert(data.msg);
                            }
                        },
                        error: function (data) {
                            alert(data.Message);
                        }
                    });
                }
                return false;
            });
        });
    });

    // Редактирование данных пользователя.
    $(document).on('click', '#editUser', function (e) {
        e.preventDefault();

        var container = this.closest('tr');
        var _id = $(container).find('#idUser').data('id');

        $("#loading").show();

        var url = "/Account/EditUser/" + _id;

        $('#myModalBodyDiv1').load(url, function () {
            $("#loading").hide();
            $('#myModal1').modal('show');
            $('.modal-title').html('Редактирование пользователя');

            var $form = $('#myForm');
            $.validator.unobtrusive.parse($form);

            $('.close').on('click', function () {
                $('.modal').modal('hide');
                $('.modal-title').empty();
                $('#myModalBodyDiv1').empty();
            });

            $form.submit(function () {

                if ($form.valid()) {
                    $.ajax({
                        type: "POST",
                        url: "/Account/EditUser/",
                        data: $(this).serialize(),
                        success: function (data) {

                            if (data.result === true) {
                                $('.modal').modal('hide');
                                $(container).replaceWith(
                                    '<tr><td>' + data.fio +
                                    '</td><td>' + data.role +
                                    '</td>' +
                                    '<td><span id="idUser" hidden = "hidden"  data-id=' + data.id + '></span >' +
                                    '<span title="Подробнее" id="userDataDetails" class="ico-details icon-button">' +
                                    '</span><span title="Редактировать" id="editUser" class="ico-edit icon-button">' +
                                    '</span><span title="Удалить пользователя" id="deleteUser" class="ico-delete icon-button">' +
                                    '</span ></td >'
                                );

                                $('#myModalBodyDiv1').empty();
                            }
                            else if (data.result === false) {
                                alert('Ошибка валидации.');
                            }
                        },
                        error: function (data) {
                            alert(data);
                        }
                    });
                }
                return false;
            });

        });

    });

    // Удаление пользователя.
    $(document).on('click', '#deleteUser', function (e) {
        var _id = $(this.closest('tr')).find('#idUser').data('id');
        var el_tr = $(this).closest("tr");
        var el_tbody = $(el_tr).closest('tbody');

        if (confirm("Вы действительно хотите удалить?")) {

            $.ajax({
                type: "POST",
                url: "/Account/DeleteUserAsync/",
                data: { id: _id },
                success: function (data) {

                    if (data.result === true) {
                        //el_tbody.find(el_tr).remove();
                        $('#listUser').click();
                    }
                    else {
                        alert("Данная запись не найдена.");
                    }
                },
                error: function (data) {
                    alert('Нет ответа от сервера.');
                }
            });
        }
    });

    /* ___________Уровень пользователя___________ */

    // Открытие профиля.
    $(document).on('click', '#editUserData', function () {

        var _id = $(document).find('#editUserData').data('id');

        $("#loading").show();

        window.location.host;
        window.location.href = '/Account/EditUserData/' + _id;
    });

    /* ______________Работа с проектами______________ */

    // Список проектов
    $(document).on('click', '#projectList', function (e) {
        e.preventDefault();

        $("#loading").show();

        window.location.host;
        window.location.href = '/Home/ListProject/';

    });

    // Cоздание нового проекта.
    $(document).on('click', '#projectCreate', function (e) {
        e.preventDefault();

        $("#loading").show();

        var url = "/Home/CreateProject/";

        $('#myModalBodyDiv1').load(url, function () {

            $("#loading").hide();
            $('.modal-title').html('Новый проект');
            $('#myModal1').modal('show');

            var $form = $('#myForm');
            $.validator.unobtrusive.parse($form);

            $('.close').on('click', function () {
                $('.modal').modal('hide');
                $('.modal-title').empty();
                $('#myModalBodyDiv1').empty();
            });

            $form.submit(function () {

                if ($form.valid()) {
                    $.ajax({
                        type: "POST",
                        url: "/Home/CreateProject/",
                        data: $(this).serialize(),
                        success: function (data) {

                            if (data.result === true) {
                                $('.modal').modal('hide');
                                $(document).find('tbody').append(
                                    '<tr><td>' + data.ProjectName +
                                    '</td><td>' + data.ProjectManager +
                                    '</td><td>' + data.DateCreate +
                                    '</td><td></td>' +
                                    '<td><span hidden = "hidden" id="idProject" data-id=' + data.ProjectId + '></span >' +
                                    '<span title="Редактировать проект" id="projectEdit" class="ico-edit icon-button">' +
                                    '</span><span title="Список задач" id="projectTaskList" class="ico-tasklist icon-button"></span>' +
                                    '<span title="Удалить проект" id="projectDelete" class="ico-delete icon-button"></span></td >'
                                );

                                console.log(data);

                                $('#myModalBodyDiv1').empty();
                            }
                            else if (data.result === false) {
                                alert('Ошибка валидации.');
                            }
                        },
                        error: function (data) {
                            alert(data.msg);
                        }
                    });
                }
                return false;
            });
        });
    });

    // Редактирование проекта.
    $(document).on('click', '#projectEdit', function (e) {
        e.preventDefault();

        var container = this.closest('tr');
        var _id = $(container).find('#idProject').data('id');

        $("#loading").show();

        var url = "/Home/ProjectEdit/" + _id;

        $('#myModalBodyDiv1').load(url, function () {
            $("#loading").hide();
            $('#myModal1').modal('show');
            $('.modal-title').html('Редактировать проект');

            var $form = $('#myForm');
            $.validator.unobtrusive.parse($form);

            $('.close').on('click', function () {
                $('.modal').modal('hide');
                $('.modal-title').empty();
                $('#myModalBodyDiv1').empty();
            });

            $form.submit(function () {

                if ($form.valid()) {
                    $.ajax({
                        type: "POST",
                        url: "/Home/ProjectEdit/",
                        data: $(this).serialize(),
                        success: function (data) {

                            if (data.result === true) {
                                $('.modal').modal('hide');
                                $(container).replaceWith(
                                    '<tr><td>' + data.ProjectName +
                                    '</td><td>' + data.ProjectManager +
                                    '</td><td>' + data.DateCreate +
                                    '</td><td>' + data.DateClose +
                                    '<td><span hidden = "hidden" id="idProject" data-id=' + data.ProjectId + '></span >' +
                                    '<span title="Редактировать проект" id="projectEdit" class="ico-edit icon-button">' +
                                    '</span><span title="Список задач" id="projectTaskList" class="ico-tasklist icon-button"></span>' +
                                    '<span title="Удалить проект" id="projectDelete" class="ico-delete icon-button"></span></td >'
                                );

                                $('#myModalBodyDiv1').empty();
                            }
                            else if (data.result === false) {
                                alert('Ошибка валидации.');
                            }
                        },
                        error: function (data) {
                            alert(data);
                            console.log(data);
                        }
                    });
                }
                return false;
            });

        });
    });

    // Удаление проекта.
    $(document).on('click', '#projectDelete', function (e) {
        var container = this.closest('tr');
        var _id = $(container).find('#idProject').data('id');

        if (confirm("Вы действительно хотите удалить?")) {

            $.ajax({
                type: "POST",
                url: "/Home/ProjectDelete/",
                data: { id: _id },
                success: function (data) {

                    if (data.result === true) {
                        $(container).remove();
                        $('#taskRes').empty();

                    }
                    else {
                        alert("Данная запись не найдена.");
                    }
                },
                error: function (data) {
                    alert(data.msg);
                }
            });
        }
    });

    /* ______________Работа с задачами______________ */

    // Список задач по проекту
    $(document).on('click', '#projectTaskList', function (e) {
        e.preventDefault();

        var container = this.closest('td');
        var _id = $(container).find('#idProject').data('id');

        $("#loading").show();

        $.ajax({
            url: "/Home/ListTask/",
            data: { id: _id },
            success: function (data) {
                $('#taskRes').html(data);
                $("#loading").hide();
            },
            error: function (data) {
                alert(data);
                console.log(data);
            }
        });
    });

    // Создание задачи по проекту.
    $(document).on('click', '#createTask', function (e) {
        e.preventDefault();

        var container = this.closest('tr');
        var _id = $(this).data('id');

        $("#loading").show();

        var url = "/Home/CreateTask/" + _id;

        $('#myModalBodyDiv1').load(url, function () {
            $("#loading").hide();
            $('#myModal1').modal('show');
            $('.modal-title').html('Новая задача');

            var $form = $('#myForm');
            $.validator.unobtrusive.parse($form);

            $('.close').on('click', function () {
                $('.modal').modal('hide');
                $('.modal-title').empty();
                $('#myModalBodyDiv1').empty();
            });

            $form.submit(function () {

                if ($form.valid()) {
                    $.ajax({
                        type: "POST",
                        url: "/Home/CreateTask/",
                        data: $(this).serialize(),
                        success: function (data) {

                            if (data.result === true) {
                                $('.modal').modal('hide');
                                $('#taskRes').find('tbody').append(
                                    '<tr><td>' + data.taskName +
                                    '</td><td>' + data.taskType +
                                    '</td><td>' + data.description +
                                    '</td><td>' + data.taskPriority +
                                    '</td><td>' + data.taskUser +
                                    '</td><td>' + data.taskStatus +
                                    '</td><td>' + data.DateCreate +
                                    '</td><td></td><td>' +
                                    '<span hidden = "hidden" id="idTask" data-id=' + data.taskId + '></span >' +
                                    '<span title="Редактировать задачу" id="TaskEdit" class="ico-edit icon-button">' +
                                    '</span><span title="Удалить задачу" id="TaskDelete" class="ico-delete icon-button"></span></td >'
                                );

                                console.log(data);

                                $('#myModalBodyDiv1').empty();
                            }
                            else if (data.result === false) {
                                alert('Ошибка валидации.');
                            }
                        },
                        error: function (data) {
                            console.log(data);
                        }
                    });
                }
                return false;
            });
        });
    });

    // Редактирование задачи.
    $(document).on('click', '#TaskEdit', function (e) {
        e.preventDefault();

        var container = this.closest('tr');
        var _id = $(container).find('#idTask').data('id');

        $("#loading").show();

        var url = "/Home/TaskEdit/" + _id;

        $('#myModalBodyDiv1').load(url, function () {
            $("#loading").hide();
            $('#myModal1').modal('show');
            $('.modal-title').html('Редактировать задачу');

            var $form = $('#myForm');
            $.validator.unobtrusive.parse($form);

            $('.close').on('click', function () {
                $('.modal').modal('hide');
                $('.modal-title').empty();
                $('#myModalBodyDiv1').empty();
            });

            $form.submit(function () {

                if ($form.valid()) {
                    $.ajax({
                        type: "POST",
                        url: "/Home/TaskEdit/",
                        data: $(this).serialize(),
                        success: function (data) {

                            if (data.result === true) {
                                $('.modal').modal('hide');
                                $(container).replaceWith(
                                    '<tr><td>' + data.taskName +
                                    '</td><td>' + data.taskType +
                                    '</td><td>' + data.description +
                                    '</td><td>' + data.taskPriority +
                                    '</td><td>' + data.taskUser +
                                    '</td><td>' + data.taskStatus +
                                    '</td><td>' + data.DateCreate +
                                    '</td><td>' + data.DateClose +
                                    '</td>' +
                                    '<td><span hidden = "hidden" id="idTask" data-id=' + data.taskId + '></span >' +
                                    '<span title="Редактировать задачу" id="TaskEdit" class="ico-edit icon-button">' +
                                    '</span><span title="Удалить задачу" id="TaskDelete" class="ico-delete icon-button"></span></td >'
                                );

                                $('#myModalBodyDiv1').empty();
                            }
                            else if (data.result === false) {
                                alert('Ошибка валидации.');
                            }
                        },
                        error: function (data) {
                            alert(data);
                            console.log(data);
                        }
                    });
                }
                return false;
            });

        });
    });

    // Удаление задачи.
    $(document).on('click', '#TaskDelete', function (e) {
        var _id = $(this.closest('tr')).find('#idTask').data('id');
        var el_tr = $(this).closest("tr");
        var el_tbody = $(el_tr).closest('tbody');

        if (confirm("Вы действительно хотите удалить?")) {

            $.ajax({
                type: "POST",
                url: "/Home/TaskDelete/",
                data: { id: _id },
                success: function (data) {

                    if (data.result === true) {
                        el_tbody.find(el_tr).remove();
                    }
                    else {
                        alert("Данная запись не найдена.");
                    }
                },
                error: function (data) {
                    alert('Нет ответа от сервера.');
                    console.log(data);
                }
            });
        }
    });

});

/* ______________Профиль______________ */

// Маска мобильного.
$(function () {
    $("#Phone").mask("8 (999) 999-99-99");
});

// Маска Дня Рождения.
$(function () {
    $("#Birthday").mask("99.99.9999 г.");
});