// Copyright 2010 Geoffrey 'Phogue' Green
// 
// http://www.phogue.net
//  
// This file is part of PRoCon Frostbite.
// 
// PRoCon Frostbite is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// PRoCon Frostbite is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the 
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with PRoCon Frostbite.  If not, see <http://www.gnu.org/licenses/>.


var m_pStartPage_lblDeleteConnection = "Are you sure you want <br/>to delete this connection?";

// The below methods are called within procon to update changed data/feeds.
// see startPage.temp to format each one of these items

function fnUpdateConnectionsList(jsonConnectionData) {

	var connectionData = eval(jsonConnectionData);

	if (connectionData.length > 0) {
		jQuery("#pStartPage-lblNoConnections").hide();
		jQuery("#pStartPage-lnkCreateConnection").trigger("connectionsPopulated", [ ]);
	}
	else {
		jQuery("#pStartPage-lblNoConnections").show();
	}

	for (var i = 0; i < connectionData.length; i++) {
		
		var sRowId = 'pStartPage-tblConnections-' + connectionData[i]["safehostport"];
		
		if (jQuery('#' + sRowId).length == 0) {
			jQuery("#pStartPage-tblConnections")
				.find('tbody')
				.append(
					jQuery('<tr>')
					.unbind().mouseenter(
						function() {
							jQuery(this).find('.connectionOptions').fadeIn('fast');
						}
					).mouseleave(
						function() {
							jQuery(this).find('.connectionOptions').fadeOut('fast');
						}
					)
					.append(
						jQuery('<td>')
						.attr('id', sRowId)
						.html(connectionData[i]["html"])
					)
				);
		}
		else {

			jQuery('#' + sRowId).html(connectionData[i]["html"]);
		}
		
	}
	
	jQuery('.deleteConnectionLink').unbind().click(function() {
		
		jQuery(this).fastConfirm({
			position: "bottom",
			proceedImage: "images/online.png",
			cancelImage: "images/error.png",
			questionText: m_pStartPage_lblDeleteConnection,
			onProceed: function(trigger) {
				window.external.DeleteConnection(jQuery(trigger).attr('hostnameport'))
			}
		});
	});
}

function fnRemoveConnection(sSafeHostnamePort) {
	jQuery('#pStartPage-tblConnections-' + sSafeHostnamePort)
		.parent('tr')
		.remove();
}

//function fnUpdateConnectionsSummary(connectionSummaryData) { 
//	jQuery('#pStartPage-lblConnectionsSummary').html(connectionSummaryData);
//}

function UpdateRssFeed(rssData) { 
	jQuery("#rssFeed").hide().html(rssData).slideDown(1000, "easeOutExpo");
}

function UpdateRssDonationFeed(rssData) { 
	jQuery("#donateWall").hide().html(rssData).slideDown(1000, "easeOutExpo");
}

function UpdateRssMonthlySummaryFeed(rssData) { 
	jQuery("#monthlySummary").hide().html(rssData).slideDown(1000, "easeOutExpo");
}

function LoadingRssFeed() {
	var loadingHtml = "<p class='loading'><img src='images/rss-animated.gif' width='16px' height='16px'> Loading...</p>";

	jQuery("#rssFeed").html(loadingHtml);
	jQuery("#donateWall").html(loadingHtml);
	jQuery("#monthlySummary").html(loadingHtml);
}

// TO DO: Cleanup

function limitChars(textid, limit, infodiv)
{
	var text = jQuery('#'+textid).val();	
	var textlength = text.length;
	if(textlength > limit)
	{
		jQuery('#' + infodiv).html('You cannot write more then '+limit+' characters!');
		jQuery('#'+textid).val(text.substr(0,limit));
		return false;
	}
	else
	{
		jQuery('#' + infodiv).html('You have '+ (limit - textlength) +' characters left.');
		return true;
	}
}
function displayVals() {
      var t3 = jQuery("#t3").val();
      var amount = jQuery("#amount").val();
      if(t3 != 0){
	    jQuery('#a3').val(amount);
	    jQuery('#p3').val(1);
		jQuery('#cmd').val('_xclick-subscriptions')
	  }else{
	  	jQuery('#a3').val(0);
	  	jQuery('#p3').val(0);
	  	jQuery('#cmd').val('_donations');
	  }
	  if( !t3 ) jQuery('#cmd').val('_donations');
	  
}
 
jQuery(function(){
 	jQuery('#donor_comment').keyup(function(){
 		limitChars('donor_comment', 199, 'charinfo');
 	})
 
 	jQuery("#amount").change(displayVals);
 	jQuery("#t3").change(displayVals);
 	displayVals();
 	
 	//var WallOps1 = '<p class="show_onwall" id="wallops"><input type="hidden" name="item_number" value="0:1" /></p>';
 	//var WallOps2 = '<p class="show_onwall" id="wallops"><label for="show_onwall">Show on Wall:</label><br /><select name="item_number"><option value="1:1">Amount, Details &amp; Comments</option><option value="2:1">Details &amp; Comments Only</option></select></p>';
 
 	if( jQuery('#pStartPage-chkRecognize').is(':checked') == false){ 
 		jQuery('#wallinfo').hide();
		jQuery('#wallops input').val('0:1');
 	}
 	
 	jQuery("#pStartPage-chkRecognize").click(function(){
 		jQuery("#wallinfo").toggle('slow');
		
		if( jQuery('#pStartPage-chkRecognize').is(':checked') == false){ 
			jQuery("#wallops option[value=0:1]").attr("selected", true);
		}
 	})

	// Connections
 	jQuery("#pStartPage-lnkCreateConnection").click(function(){
	
		jQuery('#pStartPage-lnkConnectCreateConnection').removeClass('positive').addClass('disabled');
		jQuery("#hostname").val('');
		jQuery("#port").val('48888');
		jQuery("#username").val('');
		jQuery("#password").val('');
		
 		jQuery("#pStartPage-divCreateConnection").toggle('slow');
		jQuery("#pStartPage-lnkCreateConnection").fadeOut();
		
		jQuery("#hostname, #port, #password").unbind('keyup');
		jQuery("#hostname, #port, #password").keyup( function(e) {
			if (jQuery("#hostname").val().length > 0 && jQuery("#port").val().length > 0 && jQuery("#password").val().length > 0) {
				
				jQuery('#pStartPage-lnkConnectCreateConnection').unbind('click');
				jQuery("#pStartPage-lnkConnectCreateConnection").click(function(){
				
					jQuery("#pStartPage-divCreateConnection").toggle('slow');
					jQuery("#pStartPage-lnkCreateConnection").fadeIn();
				
					window.external.CreateConnection(jQuery("#hostname").val(), jQuery("#port").val(), jQuery("#username").val(), jQuery("#password").val());
				});
				
				jQuery('#pStartPage-lnkConnectCreateConnection').removeClass('disabled').addClass('positive');
				
				// If enter was pressed
				if (e.keyCode == 13) {
					jQuery("#pStartPage-lnkConnectCreateConnection").click();
				}
			}
			else {
				jQuery('#pStartPage-lnkConnectCreateConnection').unbind('click');
				
				jQuery('#pStartPage-lnkConnectCreateConnection').removeClass('positive').addClass('disabled');
			}
		});
		
		jQuery("#pStartPage-lnkCancelCreateConnection").unbind('click');
		jQuery("#pStartPage-lnkCancelCreateConnection").click(function(){
			jQuery("#pStartPage-divCreateConnection").toggle('slow');
			jQuery("#pStartPage-lnkCreateConnection").fadeIn();
		});
 	});
	
	// Tabs
	jQuery('#container-5 ul').tabs({ fxFade: true, fxSpeed: 'fast' });

});

function periodical() {
	jQuery('#pStartPage-lnkCreateConnection').effect('bounce', {}, 500);
}

jQuery(document).ready(function() {
	var shake = setInterval(periodical, 5000);

	jQuery("#pStartPage-divCreateConnection").hide();
	jQuery("#pStartPage-lblNoConnections").hide();

	jQuery("#port").jStepper({minValue:1, maxValue:65535, allowDecimals:false, disableNonNumeric:true});

    jQuery("input[type='text'], input[type='password'], textarea").addClass("idleField");
        jQuery("input[type='text'], input[type='password'], textarea").focus(function(){
            jQuery(this).addClass("activeField").removeClass("idleField");
    }).blur(function(){
            jQuery(this).removeClass("activeField").addClass("idleField");
    });

	jQuery('#pStartPage-lnkCreateConnection').hide().css('display','').fadeIn(600);
	jQuery('#pStartPage-lnkCreateConnection').bind("connectionsPopulated click", function(event) {
		clearInterval(shake);
	});	

	//jQuery('#pStartPage-tblPackages').dataTable();
	/*
	jQuery('#pStartPage-tblPackages').dataTable( {
		"aoColumnDefs": [ 
			{ "bSearchable": false, "bVisible": false, "aTargets": [ 2 ] },
			{ "bVisible": false, "aTargets": [ 3 ] }
		] } );
	*/
	
	jQuery('#pStartPage-tblPackages').dataTable( {
		"aoColumnDefs": [ 
			{ "bSortable": false, "bSearchable": false, "aTargets": [ 0 ] },
			{ "bSearchable": false, "bVisible": false, "aTargets": [ 8, 9, 11, 14, 15, 16, 17 ] },
			{ "bVisible": false, "aTargets": [ 1, 10, 12, 13 ] }
		],
		"bAutoWidth": false,
		"bPaginate": false,
		"bLengthChange": false,
		"bInfo": false } );
	
	// Alert procon the document is ready for updates.
	window.external.DocumentReady();
});




var promotionsSet = false;
function UpdatePromotions(sPromotionsList) {

	if (promotionsSet == false) {

		var aPackageList = eval(sPromotionsList);

		for (var i = 0; i < aPackageList.length; i++) {
			jQuery('.newsticker-jcarousellite ul').append('<li><a onClick="window.external.HREF(\'' + aPackageList[i].link + '\')"><img src="' + aPackageList[i].image + '" alt="' + aPackageList[i].name + '"></a></li>');
		}

		jQuery('.newsticker-jcarousellite').jCarouselLite({
			//vertical: true,
			hoverPause:true,
			visible: 1,
			auto:20000,
			speed:2000
		});
	
		promotionsSet = true;
	}
}










/* Localization */

function fnSetLocalization(sId, sLocalText) {
	jQuery('#' + sId).html(sLocalText);
}

function fnSetVariableLocalization(sVariable, sLocalText) {
	window[sVariable] = sLocalText;
}

function fnSetTableHeadersLocalization(sId, sLocalText) {

	var aLocalTableHeaderTexts = eval(sLocalText);
	var oSettings = jQuery('#' + sId).dataTable({ "bRetrieve": true }).fnSettings();
	//var oTable = ;

	for (var i = 0; i < aLocalTableHeaderTexts.length; i++) {
		oSettings.aoColumns[i].nTf.innerHTML = oSettings.aoColumns[i].nTh.innerHTML = aLocalTableHeaderTexts[i];
	}
}



















// V Image
// H UID
// V Type - Plugin
// V Name
// V Version - *new start next to it if it's updated + green text
// V Last update - time stamp of the .zip file
// V Installed/Updated
// V Downloads
// H ImageLink
// H ForumLink
// H Author
// H Website
// H Tags
// H Description

var aUidTableRows = new Array();

function fnGetPackageTypeHTML(sPackageType, sPackageTypeLocalized) {
	return '<img src="images/datatables/' + sPackageType.toLowerCase() + '.png"> ' + sPackageTypeLocalized;
}

/*
public static int PACKAGE_STATUSCODE_NOTINSTALLED = 0;
public static int PACKAGE_STATUSCODE_DOWNLOADBEGIN = 1;
public static int PACKAGE_STATUSCODE_DOWNLOADSUCCESS = 2;
public static int PACKAGE_STATUSCODE_DOWNLOADFAIL = 3;
public static int PACKAGE_STATUSCODE_INSTALLBEGIN = 4;
public static int PACKAGE_STATUSCODE_INSTALLSUCCESS = 5;
public static int PACKAGE_STATUSCODE_INSTALLFAIL = 6;
public static int PACKAGE_STATUSCODE_INSTALLQUEUED = 7;
public static int PACKAGE_STATUSCODE_PACKAGEUPDATED = 8;
*/


function fnGetStatusHTML(iStatusCode, sStatusLocalized, bAnimateIt) {

	var sStatusHTML = '';

	if ((iStatusCode == 1 || iStatusCode == 4) && bAnimateIt == true) {
		sStatusHTML = '<img src="images/ajax-loader.gif" width="16" height="16"/>';
	}
	else if ((iStatusCode == 1 || iStatusCode == 4) && bAnimateIt == false) {
		sStatusHTML = '<img src="images/information-button.png" width="16" height="16"/>';
	}
	else if (iStatusCode == 2 || iStatusCode == 5 || iStatusCode == 7) {
		sStatusHTML = '<img src="images/online.png">';
	}
	else if (iStatusCode == 3 || iStatusCode == 6) {
		sStatusHTML = '<img src="images/error.png">';
	}
	else if (iStatusCode == 8) {
		sStatusHTML = '<img src="images/datatables/new.png">';
	}

	sStatusHTML += ' ' + sStatusLocalized;

	return sStatusHTML;
}

function UpdatePackageList(sPackageList) {

	var aPackageList = eval(sPackageList);

	var oTable = jQuery('#pStartPage-tblPackages').dataTable();

	for (var row = 0; row < aPackageList.length; row++) {

		if (aUidTableRows[aPackageList[row]["uid"]] == undefined) {
			aUidTableRows[aPackageList[row]["uid"]] = jQuery('#pStartPage-tblPackages').dataTable().fnAddData(
				[
					"<img src='images/datatables/details_open.png'>",
					aPackageList[row]["uid"],
					fnGetPackageTypeHTML(aPackageList[row]["type"], aPackageList[row]["type_loc"]),
					aPackageList[row]["name"],
					aPackageList[row]["version"],
					aPackageList[row]["lastupdate"],
					fnGetStatusHTML(aPackageList[row]["statuscode"], aPackageList[row]["status"], true),
					aPackageList[row]["downloads"],
					aPackageList[row]["imagelink"], // not searchable
					aPackageList[row]["forumlink"], // not searchable
					aPackageList[row]["author"],
					aPackageList[row]["website"], // not searchable
					aPackageList[row]["tags"],
					aPackageList[row]["description"],
					aPackageList[row]["statuscode"],
					aPackageList[row]["filesize"],
					"", // Layer Install Status
					"" // Install Package
				]
				)[0];
				
			var oNode = jQuery('#pStartPage-tblPackages').dataTable().fnGetNodes(aUidTableRows[aPackageList[row]["uid"]]);
				
			jQuery(oNode).click( function () {
			
				var drop_image = jQuery(this).find("td:first img");
				var aData = oTable.fnGetData( this );
				
				if ( drop_image.attr("src").match('details_close') ) {
					drop_image.attr("src", "images/datatables/details_open.png");
					oTable.fnClose(this);
				}
				else {
					drop_image.attr("src", "images/datatables/details_close.png");
					//drop_image.src = "../examples_support/details_close.png";
					oTable.fnOpen(this, fnFormatDetails(oTable, this), aData[1] + '_details' );
				}
			} );
		}
		else {

			jQuery('#pStartPage-tblPackages').dataTable().fnUpdate(
				[
					"",
					aPackageList[row]["uid"],
					fnGetPackageTypeHTML(aPackageList[row]["type"], aPackageList[row]["type_loc"]),
					aPackageList[row]["name"],
					aPackageList[row]["version"],
					aPackageList[row]["lastupdate"],
					fnGetStatusHTML(aPackageList[row]["statuscode"], aPackageList[row]["status"], true),
					aPackageList[row]["downloads"],
					aPackageList[row]["imagelink"], // not searchable
					aPackageList[row]["forumlink"], // not searchable
					aPackageList[row]["author"],
					aPackageList[row]["website"], // not searchable
					aPackageList[row]["tags"],
					aPackageList[row]["description"],
					aPackageList[row]["statuscode"],
					aPackageList[row]["filesize"],
					"", // Layer Install Status
					"" // Install Package
				],
				aUidTableRows[aPackageList[row]["uid"]],
				0
			);
		}
	}
}

function UpdatePackageLocalInstallStatus(sPackage, iStatusCode, sStatus) {

	var aPackage = eval("[" + sPackage + "]")[0];

	if (aUidTableRows[aPackage["uid"]] != undefined) {

		var oTable = jQuery('#pStartPage-tblPackages').dataTable();
		var oNode = oTable.fnGetNodes(aUidTableRows[aPackage["uid"]]);

		oTable.fnUpdate(
			fnGetStatusHTML(iStatusCode, sStatus, true),
			aUidTableRows[aPackage["uid"]],
			6
		);
		
		oTable.fnUpdate(
			iStatusCode,
			aUidTableRows[aPackage["uid"]],
			14
		);
		
		oTable.find('.' + aPackage["uid"] + '_details').html(fnFormatDetails(oTable, oNode));
		
		//oTable.fnOpen(this, fnFormatDetails(oTable, oNode), 'details' );
		//fnFormatDetails(oTable, oNode);
	}
}

function AppendPackageRemoteInstallStatus(sPackage, sHostNamePort, iStatusCode, sStatus) {
	var aPackage = eval("[" + sPackage + "]")[0];
	
	if (aUidTableRows[aPackage["uid"]] != undefined) {
		var oTable = jQuery('#pStartPage-tblPackages').dataTable();
		var oNode = oTable.fnGetNodes(aUidTableRows[aPackage["uid"]]);
		var aData = oTable.fnGetData( oNode );
		
		oTable.fnUpdate(
			aData[16] + fnGetStatusHTML(iStatusCode, '<b>[' + sHostNamePort + ']</b> ' + sStatus + '<br/>', false),
			aUidTableRows[aPackage["uid"]],
			16
		);
		
		oTable.find('.' + aPackage["uid"] + '_details').html(fnFormatDetails(oTable, oNode));
	}
}

/*
<th>Open/Close</th> 	0
<th>UID</th> 			1
<th>Type</th> 			2
<th>Name</th> 			3
<th>Version</th> 		4
<th>Last Update</th> 	5
<th>Status</th> 		6
<th>Downloads</th> 		7
<th>Image Link</th> 	8
<th>Forum Link</th> 	9
<th>Author</th> 		10
<th>Website</th> 		11
<th>Tags</th> 			12
<th>Description</th> 	13
<th>Status Code</th> 	14
<th>Package Size</th>	15
<th>Layer Install Status</th>	16
<th>Install Package</th>	17
*/

/* Formating function for row details */
function fnFormatDetails(oTable, nTr)
{
	var aData = oTable.fnGetData( nTr );
	var oSettings = oTable.fnSettings();
	var sOut = '<div class="package_uid_details">';

	sOut += '<h1>' + aData[3] + '</h1>';

	if (aData[8].length > 0) {
		sOut += '<img src="' + aData[8] + '" class="package_uid_details_logo"/>';
	}
	
	//sOut += '<p>';
	sOut += '<span class="package_uid_details_detail-title">' + oSettings.aoColumns[10].nTh.innerHTML + ':</span> <a href="#" onClick="window.external.HREF(\'' + aData[11] + '\')">' + aData[10] + '</a><br/>';
	sOut += '<span class="package_uid_details_detail-title"><a href="#" onClick="window.external.HREF(\'' + aData[9] + '\')">' + oSettings.aoColumns[9].nTh.innerHTML + '</a></span><br/>';
	
	sOut += '<span class="package_uid_details_detail-title">' + oSettings.aoColumns[13].nTh.innerHTML + '</span><br/>' + aData[13] + '<br/>';
	//sOut += '<span class="package_uid_details_detail-title">' + oSettings.aoColumns[13].nTh.innerHTML + '</span><br/>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.<br/>';
	
	// Only show the uninstall button if the package is currentely uninstalled or updated
	// IF statuscode == PackageManager.PACKAGE_STATUSCODE_NOTINSTALLED
	if (aData[14] == 0 || aData[14] == 8) {
		sOut += '<div class="buttons"><a href="#" class="positive" onClick="window.external.InstallPackage(\'' + aData[1] + '\')"><img src="images/datatables/package_download.png" alt=""/>' +  oSettings.aoColumns[17].nTh.innerHTML + ' (' +  aData[15] + ')</a></div>';
	}
	else if (aData[16].length > 0) {
		sOut += '<p><span class="package_uid_details_detail-title">' + oSettings.aoColumns[16].nTh.innerHTML + '...</span><br/>' + aData[16] + '<p/>';
	}
	
	sOut += '<div>';
	
	return sOut;
}









