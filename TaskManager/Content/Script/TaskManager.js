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

    /* ______________Работа с пользователями______________ */

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

    // Редактирование данных пользователя.
    $(document).on('click', '#editUserData', function (e) {
        e.preventDefault();

        var container = this.closest('tr');
        var _id = $(container).find('#idUser').data('id');

        $("#loading").show();

        var url = "/Account/EditUserData/" + _id;

        $('#myModalBodyDiv1').load(url, function () {
            $("#loading").hide();
            $('#myModal1').modal('show');
            $('.modal-title').html('Редактирование информации пользователя');

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
                        url: "/Account/EditUserData/",
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
                                    '</span><span title="Редактировать" id="editUserData" class="ico-edit icon-button"></span></td >'
                                );

                                console.log(data);

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

    // Удаление задачи. Пока что не работает из-за отношений таблиц.
    $(document).on('click', '#deleteUser', function (e) {
        var _id = $(this.closest('tr')).find('#idUser').data('id');
        var el_tr = $(this).closest("tr");
        var el_tbody = $(el_tr).closest('tbody');

        if (confirm("Вы действительно хотите удалить?")) {

            $.ajax({
                type: "POST",
                url: "/Home/DeleteUser/",
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
                }
            });
        }
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
                                    '<span title="Редактировать проект" id="getProjectEdit" class="ico-edit icon-button">' +
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
                            alert(data.Message);
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
                                    '<span title="Редактировать проект" id="getProjectEdit" class="ico-edit icon-button">' +
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
                            alert(data.Message);
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
                                    '</td><td>' +
                                    '</td><td></td>' +
                                    '<td><span hidden = "hidden" id="idTask" data-id=' + data.taskId + '></span >' +
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
                            alert(data.Message);
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
                                    '</td><td></td>' +
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
                            alert(data.Message);
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
                // передача в качестве объекта
                // поля будут закодированые через encodeURIComponent автоматически
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
                }
            });
        }
    });

});