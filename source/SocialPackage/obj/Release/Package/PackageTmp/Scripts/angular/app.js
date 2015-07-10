'use strict'

var app = angular.module('SocialPackage', ['SocialPackage.controllers', 'SocialPackage.directives', 'SocialPackage.filters', 'SocialPackage.services', 'angularFileUpload', "ui.bootstrap", "ui.date", "textAngular"]);

app.run(['$rootScope', function ($rootScope) {
    $rootScope.tempDefers = [];
    $rootScope.str = 234;

    $rootScope.getFormatedDate = function (date) {
        moment.locale("ru");
        return moment(date, "MMM YYYY").format("MM-YYYY");
    }

    $rootScope.getDateString = function (date) {
        var months = ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
   "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"];
        return months[new Date(date).getMonth()] + " " + new Date(date).getFullYear();
    };

}]);