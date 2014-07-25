ko.bindingHandlers.editor = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		if (!ko.isWriteableObservable(valueAccessor())) {
			throw 'valueAccessor must be writeable and observable';
		}

		// Get custom configuration object from the 'wysiwygConfig' binding, more settings here... http://www.tinymce.com/wiki.php/Configuration
		var options = allBindings.has('editorConfig') ? allBindings.get('editorConfig') : {},

			// Set up a minimal default configuration
			defaults = {
				'browser_spellcheck': $(element).prop('spellcheck'),

				"language":"zh_CN",
				'language_url': '/Scripts/tinymce/langs/zh_CN.js',

				"plugins": "fullscreen code link paste textcolor sigimage sigdownload inserthtml",
				"toolbar": "code fullscreen | styleselect | alignleft aligncenter alignright alignjustify | bold italic forecolor backcolor | bullist numlist | link inserthtml sigimage sigdownload",
				'menubar': false,
				'statusbar': false,
				'relative_urls': false,
				"convert_urls": true,
				'content_css': '/App_Themes/Classic/tinymce/content.css',
				'setup': function (editor) {
					// Ensure the valueAccessor state to achieve a realtime responsive UI.
					editor.on('change keyup nodechange', function (args) {
						// Update the valueAccessor
						valueAccessor()(editor.getContent());
					});
				}
			};

		// Apply custom configuration over the defaults
		defaults = $.extend(defaults, options);

		// Ensure the valueAccessor's value has been applied to the underlying element, before instanciating the tinymce plugin
		$(element).text(valueAccessor()());

		// Tinymce will not be able to calculate the textarea height without this delay
		setTimeout(function () {
			if (!element.id) {
				element.id = tinymce.DOM.uniqueId();
			}
			tinyMCE.init(defaults);
			tinymce.execCommand("mceAddEditor", true, element.id);
		}, 50);

		// To prevent a memory leak, ensure that the underlying element's disposal destroys it's associated editor.
		ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
			tinyMCE.get($(element).attr('id')).remove();
		});
	},
	update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		// Implement the 'value' binding
		return ko.bindingHandlers['value'].update(element, valueAccessor, allBindings, viewModel, bindingContext);
	}
};
