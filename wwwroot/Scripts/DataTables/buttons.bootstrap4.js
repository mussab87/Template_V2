// ***********************************************************************
// Assembly         : Edu
// Author           : Ali Aljarrah && Walid Azouzi
// Created          : 02-23-2020
//
// Last Modified By : Ali Aljarrah && Walid Azouzi
// Last Modified On : 10-15-2019
// ***********************************************************************
// <copyright file="buttons.bootstrap4.js" company="Expert Systems">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
/*! Bootstrap integration for DataTables' Buttons
 * ©2016 SpryMedia Ltd - datatables.net/license
 */

(function( factory ){
	/// <summary>
	/// </summary>
	/// <param name="factory">The factory.</param>
	if ( typeof define === 'function' && define.amd ) {
		// AMD
		define( ['jquery', 'datatables.net-bs4', 'datatables.net-buttons'], function ( $ ) {
			/// <summary>
			/// </summary>
			/// <param name="$">The $.</param>
			return factory( $, window, document );
		} );
	}
	else if ( typeof exports === 'object' ) {
		// CommonJS
		module.exports = function (root, $) {
			/// <summary>
			/// </summary>
			/// <param name="root">The root.</param>
			/// <param name="$">The $.</param>
			if ( ! root ) {
				root = window;
			}

			if ( ! $ || ! $.fn.dataTable ) {
				$ = require('datatables.net-bs4')(root, $).$;
			}

			if ( ! $.fn.dataTable.Buttons ) {
				require('datatables.net-buttons')(root, $);
			}

			return factory( $, root, root.document );
		};
	}
	else {
		// Browser
		factory( jQuery, window, document );
	}
}(function( $, window, document, undefined ) {
/// <summary>
/// </summary>
/// <param name="$">The $.</param>
/// <param name="window">The window.</param>
/// <param name="document">The document.</param>
/// <param name="undefined">The undefined.</param>
'use strict';
var DataTable = $.fn.dataTable;

$.extend( true, DataTable.Buttons.defaults, {
	dom: {
		container: {
			className: 'dt-buttons btn-group'
		},
		button: {
			className: 'btn btn-secondary'
		},
		collection: {
			tag: 'div',
			className: 'dt-button-collection dropdown-menu',
			button: {
				tag: 'a',
				className: 'dt-button dropdown-item'
			}
		}
	}
} );

DataTable.ext.buttons.collection.className += ' dropdown-toggle';

return DataTable.Buttons;
}));
