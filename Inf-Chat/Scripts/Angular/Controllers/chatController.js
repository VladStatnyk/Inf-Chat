'use strict';
app.controller('chatController', ['$scope', '$location', '$window' ,'authService', 'chatService', function ($scope, $location, $window, authService, chatService) {
    
    $scope.chatHub = $.connection.chatHub;

    $scope.user = {
        Name: "",
        Message: ""
    };
    $scope.messages = [];
    $scope.notifications = [];

    $scope.SendMessage = function () {
        $.connection.hub.start({ transport: 'longPolling' }).done(function () {
            $scope.chatHub.server.sendMessage($scope.user.Message);
            $scope.user.Message = '';
        });  
    }

    $scope.chatHub.client.broadcastMessage = function (userName, message, date) {
        $scope.messages.push({ AuthorName: userName, Body: message, Date: date });
        $scope.$apply();
    };

    $scope.chatHub.client.userDisconnect = function (userName) {
        $scope.notifications.push("The " + userName + " left the chat!");
        $scope.$apply();
    }

    $scope.chatHub.client.userConnect = function (userName) {
        $scope.notifications.push("The " + userName + " joined the chat!");
        $scope.$apply();
    }

    $scope.Init = function () {
        $scope.user.Name = authService.authentication.userName;
        chatService.getMessages().then(function (responce) {
            $scope.messages = responce;
        });
    }

    $scope.Init();

    $scope.ScrollToBottom = function (index) {
        if (index == $scope.messages.length - 1) {
            //var h = angular.element(document.querySelector('#messages-container'))[0].offsetHeight;
            //window.scrollTo(1500, 0);
        }
    }

    $scope.StartConnection = function () {
        if (!window.localStorage.getItem("ls.authorizationData")) return;

        var token = window.localStorage.getItem("ls.authorizationData").split(":")[1].split(",")[0].substring(1);
        token = token.substring(0, token.length - 1);
        document.cookie = "BearerToken=" + token;
        $.connection.hub.start({ transport: 'longPolling' });
    }
    $scope.StartConnection();
}]);