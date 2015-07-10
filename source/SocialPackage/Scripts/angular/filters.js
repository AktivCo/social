angular.module("SocialPackage.filters", [])

  .filter('availCategories', function () {
      return function (user, Categories) {
          var out = [];
         // angular.copy(Categories, out);
          angular.forEach(Categories, function (cat, index) {
              if (user.Limit.Proezd != 0 && cat.id == 1)
                  out.push(cat);
              if (user.Limit.Fitnes != 0 && cat.id == 2)
                  out.push(cat);
              if (user.Limit.CultureEvents != 0 && cat.id == 3)
                  out.push(cat);
          })
          return out;
      }
  })