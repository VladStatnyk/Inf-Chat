'use strict';
app.factory('chatService', ['$http', '$q', 'localStorageService',
    function ($http, $q, localStorageService) {

        var chatServiceFactory = {};

        chatServiceFactory.getMessages = function () {
            var deferred = $q.defer();

            $http.get('api/Chat/GetMessages').success(function (response) {
                deferred.resolve(response);
            }).error(function (err) {
                deferred.reject(err);
            });

            return deferred.promise;
        }

        return chatServiceFactory;
    }]);