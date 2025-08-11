// Client-side adapter for [MinimumAge(min,max)]
(function ($) {
  function yearsBetween(dob) {
    var today = new Date(), d = new Date(dob);
    if (isNaN(d)) return NaN;
    var age = today.getFullYear() - d.getFullYear();
    var m = today.getMonth() - d.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < d.getDate())) age--;
    return age;
  }

  $.validator.addMethod("minimumage", function (value, element, params) {
    if (!value) return true; // [Required] handles empties
    var age = yearsBetween(value);
    if (isNaN(age)) return false;
    return age >= params.min && age <= params.max;
  }, "Invalid age.");

  // Use server error message if provided; otherwise compose 8..80 text.
  $.validator.unobtrusive.adapters.add("minimumage", ["min", "max"], function (options) {
    var min = parseInt(options.params.min, 10),
        max = parseInt(options.params.max, 10);
    options.rules["minimumage"] = { min: min, max: max };
    options.messages["minimumage"] = options.message || ("Age must be between " + min + " and " + max + ".");
  });
})(jQuery);
