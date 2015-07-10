
angular.module("SocialPackage.directives", [])
.directive('optionsColor', function ($parse) {
    return {
        require: 'select',
        link: function (scope, elem, attrs, ngSelect) {
            // get the source for the items array that populates the select.
            var optionsSourceStr = attrs.ngOptions.split(' ').pop();
            // use $parse to get a function from the options-class attribute
            // that you can use to evaluate later.
            //getOptionsClass = $parse(attrs.optionsColor);

            scope.$watch(optionsSourceStr, function (items) {
                // when the options source changes loop through its items.
                angular.forEach(items, function (item, index) {
                    // evaluate against the item to get a mapping object for
                    // for your classes.
                    //var classes = getOptionsClass(item),
                    // also get the option you're going to need. This can be found
                    // by looking for the option with the appropriate index in the
                    // value attribute.
                    option = elem.find('option[value=' + index + ']')

                    classes = { 'color': item.Color }
                    // now loop through the key/value pairs in the mapping object
                    // and apply the classes that evaluated to be truthy.
                    //angular.forEach(classes, function (add, className) {
                    //    if (add) {
                    //        angular.element(option).addClass(className);
                    //    }
                    //});

                    angular.element(option).css(classes)
                });
            });
        }
    };
})

    .directive('datetimez', ['$http', function ($http) {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                var self = element;
                var $scope = scope;
                $http.get($scope.baseUrl + "/api/Bids/StartMonth/").success(function (data) {
                    element.datepicker({
                        format: "MM yyyy",
                        minViewMode: "months",
                        language: "ru",
                        startDate: new Date(data),
                        autoclose: true

                    });

                    element.datepicker("setDate", new Date(data));
                });



            }
        }
    }])

       .directive('datetimezForPay', [function () {
           return {
               restrict: 'A',
               require: 'ngModel',
               link: function (scope, element, attrs, ngModelCtrl) {
                   var self = element;
                   var $scope = scope;

                   element.datepicker({
                       format: "MM yyyy",
                       minViewMode: "months",
                       language: "ru",
                       autoclose: true

                   });

                   element.datepicker("setDate", new Date());




               }
           }
       }])

    .directive('fileselect', ['$rootScope', function ($rootScope) {
        return {
            restrict: 'E',
            template: '<span ><button style="z-index:1"   class="load btn btn-primary buttonTop" ng-click="s()">{{text}}</button><input type="file" nv-file-select="" uploader="uploader" style="visibility: collapse;" /></span>',
            replace: true,
            //require: 'ngModel',
            link: function (scope, element, attr, ctrl) {


                scope.text = attr['text'] || 'Загрузить';

                var fileinput = element.find('input');


                scope.s = function () {
                    fileinput.val(null);
                    fileinput[0].click();
                };

                //var listener = function () {

                //    $rootScope.Apply(function () {
                //        ctrl.$setViewValue(fileinput[0].files[0]);
                //    });
                //};
                // element.bind('change click', listener);



            }
        };
    }])

.directive('ngThumb', ['$window', function ($window) {
    var helper = {
        support: !!($window.FileReader && $window.CanvasRenderingContext2D),
        isFile: function (item) {
            return angular.isObject(item) && item instanceof $window.File;
        },
        isImage: function (file) {
            var type = '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    };

    return {
        restrict: 'A',
        template: '<canvas class="imageSize" />',
        link: function (scope, element, attributes) {
            if (!helper.support) return;

            var params = scope.$eval(attributes.ngThumb);

            if (!helper.isFile(params.file)) return;
            if (!helper.isImage(params.file)) return;

            var canvas = element.find('canvas');
            var reader = new FileReader();

            reader.onload = onLoadFile;
            reader.readAsDataURL(params.file);

            function onLoadFile(event) {
                var img = new Image();
                img.onload = onLoadImage;
                img.src = event.target.result;
            }

            function rotatedDrawImage(ctx, image, fromX, fromY, angle) {
                ctx.save();
                ctx.translate(fromX + image.width / 2, fromY + image.height / 2);
                ctx.rotate(90 * Math.PI / 180);
                ctx.translate(-(fromX + image.width / 2), -(fromY + image.height / 2));
                ctx.drawImage(image, fromX, fromY);
                ctx.restore();
            }
          


            function onLoadImage() {
                //var width = params.width || this.width / this.height * params.height;
                //var height = params.height || this.height / this.width * params.width;
                var width = this.width > 800 ? 800 : this.width;
                var height = this.width > 800 ? this.height / this.width * 800 : this.height
                canvas.attr({ width: width, height: height });
                var context = canvas[0].getContext('2d')
                
                context.drawImage(this, 0, 0, width, height); //сделать rotate
              
        
             
               
            }
        }
    };
}])
