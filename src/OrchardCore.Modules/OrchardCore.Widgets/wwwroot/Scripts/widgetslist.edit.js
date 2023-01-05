/*
** NOTE: This file is generated by Gulp and should not be edited directly!
** Any changes made directly to this file will be overwritten next time its asset group is processed by Gulp.
*/

function _toConsumableArray(arr) { return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _unsupportedIterableToArray(arr) || _nonIterableSpread(); }
function _nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); }
function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }
function _iterableToArray(iter) { if (typeof Symbol !== "undefined" && iter[Symbol.iterator] != null || iter["@@iterator"] != null) return Array.from(iter); }
function _arrayWithoutHoles(arr) { if (Array.isArray(arr)) return _arrayLikeToArray(arr); }
function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }
$(function () {
  $(document).on('click', '.add-list-widget', function (event) {
    var type = $(this).data("widget-type");
    var targetId = $(this).data("target-id");
    var htmlFieldPrefix = $(this).data("html-field-prefix");
    var createEditorUrl = $('#' + targetId).data("buildeditorurl");
    var prefixesName = $(this).data("prefixes-name");
    var parentContentType = $(this).data("parent-content-type");
    var partName = $(this).data("part-name");
    var zonesName = $(this).data("zones-name");
    var zone = $(this).data("zone");

    // Retrieve all index values knowing that some elements may have been moved / removed.
    var indexes = $('#' + targetId).closest("form").find("input[name*='Prefixes']").filter(function (i, e) {
      return $(e).val().substring(0, $(e).val().lastIndexOf('-')) === htmlFieldPrefix;
    }).map(function (i, e) {
      return parseInt($(e).val().substring($(e).val().lastIndexOf('-') + 1)) || 0;
    });

    // Use a prefix based on the items count (not a guid) so that the browser autofill still works.
    var index = indexes.length ? Math.max.apply(Math, _toConsumableArray(indexes)) + 1 : 0;
    var prefix = htmlFieldPrefix + '-' + index.toString();
    var contentTypesName = $(this).data("contenttypes-name");
    var contentItemsName = $(this).data("contentitems-name");
    $.ajax({
      url: createEditorUrl + "?id=" + type + "&prefix=" + prefix + "&prefixesName=" + prefixesName + "&contentTypesName=" + contentTypesName + "&contentItemsName=" + contentItemsName + "&zonesName=" + zonesName + "&zone=" + zone + "&targetId=" + targetId + "&parentContentType=" + parentContentType + "&partName=" + partName
    }).done(function (data) {
      var result = JSON.parse(data);
      $(document.getElementById(targetId)).append(result.Content);
      var dom = $(result.Scripts);
      dom.filter('script').each(function () {
        $.globalEval(this.text || this.textContent || this.innerHTML || '');
      });
    });
  });
  $(document).on('click', '.insert-list-widget', function (event) {
    var type = $(this).data("widget-type");
    var target = $(this).closest('.widget-template');
    var targetId = $(this).data("target-id");
    var htmlFieldPrefix = $(this).data("html-field-prefix");
    var createEditorUrl = $('#' + targetId).data("buildeditorurl");
    var prefixesName = $(this).data("prefixes-name");
    var parentContentType = $(this).data("parent-content-type");
    var partName = $(this).data("part-name");
    var zonesName = $(this).data("zones-name");
    var zone = $(this).data("zone");

    // Retrieve all index values knowing that some elements may have been moved / removed.
    var indexes = $('#' + targetId).closest("form").find("input[name*='Prefixes']").filter(function (i, e) {
      return $(e).val().substring(0, $(e).val().lastIndexOf('-')) === htmlFieldPrefix;
    }).map(function (i, e) {
      return parseInt($(e).val().substring($(e).val().lastIndexOf('-') + 1)) || 0;
    });

    // Use a prefix based on the items count (not a guid) so that the browser autofill still works.
    var index = indexes.length ? Math.max.apply(Math, _toConsumableArray(indexes)) + 1 : 0;
    var prefix = htmlFieldPrefix + '-' + index.toString();
    var contentTypesName = $(this).data("contenttypes-name");
    var contentItemsName = $(this).data("contentitems-name");
    $.ajax({
      url: createEditorUrl + "?id=" + type + "&prefix=" + prefix + "&prefixesName=" + prefixesName + "&contentTypesName=" + contentTypesName + "&contentItemsName=" + contentItemsName + "&zonesName=" + zonesName + "&zone=" + zone + "&targetId=" + targetId + "&parentContentType=" + parentContentType + "&partName=" + partName
    }).done(function (data) {
      var result = JSON.parse(data);
      $(result.Content).insertBefore(target);
      var dom = $(result.Scripts);
      dom.filter('script').each(function () {
        $.globalEval(this.text || this.textContent || this.innerHTML || '');
      });
    });
  });
  $(document).on('click', '.widget-list-delete', function () {
    var $this = $(this);
    confirmDialog(_objectSpread({}, $this.data(), {
      callback: function callback(r) {
        if (r) {
          $this.closest('.widget-template').remove();
          $(document).trigger('contentpreview:render');
        }
      }
    }));
  });
  $(document).on('change', '.widget-editor-footer label', function () {
    $(document).trigger('contentpreview:render');
  });
  $(document).on('click', '.widget-list-editor-btn-toggle', function () {
    $(this).closest('.widget-editor').toggleClass('collapsed');
  });
});