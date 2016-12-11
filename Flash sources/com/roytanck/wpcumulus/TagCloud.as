/*
		com.roytanck.wpcumulus.TagCloud
		Copyright 2009: Roy Tanck 
		
		This file is part of WP-Cumulus.

    WP-Cumulus is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    WP-Cumulus is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with WP-Cumulus.  If not, see <http://www.gnu.org/licenses/>.
*/

package com.roytanck.wpcumulus
{
	import flash.display.MovieClip;
	import flash.display.Sprite;
	import flash.display.Stage;
	import flash.display.StageAlign;
	import flash.display.StageScaleMode;
	import flash.display.LoaderInfo;
	import flash.events.Event;
	import flash.net.URLRequest;
	import flash.net.URLLoader;
	import flash.text.TextField;
	import flash.text.TextFieldAutoSize;
	import flash.text.TextFormat;
	import flash.geom.ColorTransform;
	import flash.events.MouseEvent;
	import flash.ui.ContextMenu;
	import flash.ui.ContextMenuItem;
	import flash.events.ContextMenuEvent;
	import flash.net.navigateToURL;
	import flash.net.URLRequest;
	import com.roytanck.wpcumulus.Tag;

	public class TagCloud extends MovieClip	{
		// private vars
		private var radius:Number;
		private var mcList:Array;
		private var dtr:Number;
		private var d:Number;
		private var sa:Number;
		private var ca:Number;
		private var sb:Number;
		private var cb:Number;
		private var sc:Number;
		private var cc:Number;
		private var originx:Number;
		private var originy:Number;
		private var tcolor:Number;
		private var hicolor:Number;
		private var tcolor2:Number;
		private var tspeed:Number;
		private var distr:Boolean;
		private var lasta:Number;
		private var lastb:Number;
		private var holder:MovieClip;
		private var active:Boolean;
		private var myXML:XML;
		
		public function TagCloud(){
			// settings
			var swfStage:Stage = this.stage;
			swfStage.scaleMode = StageScaleMode.NO_SCALE;
			swfStage.align = StageAlign.TOP_LEFT;
			// add context menu item
			var myContextMenu:ContextMenu = new ContextMenu();
			myContextMenu.hideBuiltInItems();
			var item:ContextMenuItem = new ContextMenuItem("WP-Cumulus by Roy Tanck and Luke Morton");
			myContextMenu.customItems.push(item);
			this.contextMenu = myContextMenu;
			item.addEventListener(ContextMenuEvent.MENU_ITEM_SELECT, menuItemSelectHandler);
			// get flashvar for text color
			tcolor = ( this.loaderInfo.parameters.tcolor == null ) ? 0x333333 : Number(this.loaderInfo.parameters.tcolor);
			tcolor2 = ( this.loaderInfo.parameters.tcolor2 == null ) ? 0x995500 : Number(this.loaderInfo.parameters.tcolor2);
			hicolor = ( this.loaderInfo.parameters.hicolor == null ) ? 0x000000 : Number(this.loaderInfo.parameters.hicolor);
			tspeed = ( this.loaderInfo.parameters.tspeed == null ) ? 1 : Number(this.loaderInfo.parameters.tspeed)/100;
			distr = ( this.loaderInfo.parameters.distr == "true" );
			
			//var categories:String = '%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Fenglish%2F%22+title%3D%22View+all+posts+filed+under+English%22%3EEnglish%3C%2Fa%3E+%2894%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Finternet%2Fflash%2F%22+title%3D%22View+all+posts+filed+under+Flash%22%3EFlash%3C%2Fa%3E+%289%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Fgadgets%2F%22+title%3D%22View+all+posts+filed+under+Gadgets%22%3EGadgets%3C%2Fa%3E+%2823%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Fideas%2F%22+title%3D%22View+all+posts+filed+under+ideas%22%3Eideas%3C%2Fa%3E+%282%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Finternet%2F%22+title%3D%22View+all+posts+filed+under+Internet%22%3EInternet%3C%2Fa%3E+%2828%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Fnederlands%2F%22+title%3D%22View+all+posts+filed+under+Nederlands%22%3ENederlands%3C%2Fa%3E+%2812%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Fpersonal%2F%22+title%3D%22View+all+posts+filed+under+Personal%22%3EPersonal%3C%2Fa%3E+%2817%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Finternet%2Fseo%2F%22+title%3D%22View+all+posts+filed+under+SEO%22%3ESEO%3C%2Fa%3E+%286%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Fsoftware%2F%22+title%3D%22View+all+posts+filed+under+Software%22%3ESoftware%3C%2Fa%3E+%2810%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Funcategorized%2F%22+title%3D%22View+all+posts+filed+under+Uncategorized%22%3EUncategorized%3C%2Fa%3E+%285%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Finternet%2Fwebdesign%2F%22+title%3D%22View+all+posts+filed+under+Web+design%22%3EWeb+design%3C%2Fa%3E+%2812%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Finternet%2Fwordpress%2F%22+title%3D%22View+all+posts+filed+under+WordPress%22%3EWordPress%3C%2Fa%3E+%284%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Finternet%2Fwordpress-plugins%2F%22+title%3D%22View+all+posts+filed+under+WordPress+plugins%22%3EWordPress+plugins%3C%2Fa%3E+%2813%29%3Cbr+%2F%3E%0A%09%3Ca+href%3D%22http%3A%2F%2Fwww.roytanck.com%2Fcategory%2Finternet%2Fwordpress-themes%2F%22+title%3D%22View+all+posts+filed+under+WordPress+themes%22%3EWordPress+themes%3C%2Fa%3E+%2813%29%3Cbr+%2F%3E%0A';

			// load or parse the data
			myXML = new XML();
			if( this.loaderInfo.parameters.mode == null )	{
				// base url
				var a:Array = this.loaderInfo.url.split("/");
				a.pop();
				var baseURL:String = a.join("/") + "/";
				// load XML file
				var XMLPath = ( this.loaderInfo.parameters.xmlpath == null ) ? baseURL + "tagcloud.xml" : this.loaderInfo.parameters.xmlpath;
				var myXMLReq:URLRequest = new URLRequest( XMLPath );
				var myLoader:URLLoader = new URLLoader(myXMLReq);
				myLoader.addEventListener("complete", xmlLoaded);
				function xmlLoaded(event:Event):void {
						myXML = XML(myLoader.data); // test with tags from XML file
						//myXML = new XML("<tags></tags>"); // test without tags
						//addCategories( categories ); // add categories
						init( myXML );
				}
			} else {
				switch( this.loaderInfo.parameters.mode ){
					case "tags":
						myXML = new XML( this.loaderInfo.parameters.tagcloud );
						break;
					case "cats":
						myXML = new XML("<tags></tags>");
						addCategories( this.loaderInfo.parameters.categories );
						break;
					default:
						myXML = new XML( this.loaderInfo.parameters.tagcloud );
						addCategories( this.loaderInfo.parameters.categories );
						break;
				}
				init( myXML );
			}
		}
		
		private function addCategories( cats:String ){
			// unescape leave spaces as '+', so we have to filter these out manually
			cats = unescape(cats).replace(/\+/g, " ");
			// use the fact that WP outputs line breaks to split the string into bits
			var cArray:Array = cats.split("<br />");
			// loop though them to find the smallest and largest 'tags'
			var smallest:Number = 9999;
			var largest:Number = 0;
			var pattern:RegExp = /\d/g;
			for( var i:Number=0; i<cArray.length-1; i++ ){
				var parts:Array = cArray[i].split( "</a>" );
				// user regular extpressions to get rid of extra stuff
				var nr:Number = Number( parts[1].match(pattern).join("") );
				largest = Math.max( largest, nr );
				smallest = Math.min( smallest, nr );
			}
			// how much must we scale the categories to match the tags?
			var scalefactor:Number = ( smallest == largest )? 7/largest : 14 / largest;
			// loop through them again and add to XML
			for( i=0; i<cArray.length-1; i++ ){
				parts = cArray[i].split( "</a>" );
				nr = Number( parts[1].match(pattern).join("") );
				var node:String = "<a style=\"" + ((nr*scalefactor)+8) + "\"" + parts[0].substr( parts[0].indexOf("<a")+2 ) + "</a>";
				myXML.appendChild( node );
			}
		}
		
		private function init( o:XML ):void {
			// set some vars
			radius = 150;
			dtr = Math.PI/180;
			d=300;
			sineCosine( 0,0,0 );
			mcList = [];
			active = false;
			lasta = 1;
			lastb = 1;
			// create holder mc, center it		
			holder = new MovieClip();
			addChild(holder);
			resizeHolder();
			// loop though them to find the smallest and largest 'tags'
			var largest:Number = 0;
			var smallest:Number = 9999;
			for each( var node:XML in o.a ){
				var nr:Number = getNumberFromString( node["@style"] );
				largest = Math.max( largest, nr );
				smallest = Math.min( smallest, nr );
			}
			// create movie clips
			for each( var node2:XML in o.a ){
				// figure out what color it should be
				var nr2:Number = getNumberFromString( node2["@style"] );
				var perc:Number = ( smallest == largest ) ? 1 : (nr2-smallest) / (largest-smallest);
				// create mc
				var col:Number = ( node2["@color"] == undefined ) ? getColorFromGradient( perc ) : Number( node2["@color"] );
				var hicol:Number = ( node2["@hicolor"] == undefined ) ? ( ( hicolor == tcolor ) ? getColorFromGradient( perc ) : hicolor ) : Number( node2["@hicolor"] );
				var mc:Tag = new Tag( node2, col, hicol );
				holder.addChild(mc);
				// store reference
				mcList.push( mc );
			}
			// distribute the tags on the sphere
			positionAll();
			// add event listeners
			addEventListener(Event.ENTER_FRAME, updateTags);
			stage.addEventListener(Event.MOUSE_LEAVE, mouseExitHandler);
			stage.addEventListener(MouseEvent.MOUSE_MOVE, mouseMoveHandler);
			stage.addEventListener(Event.RESIZE, resizeHandler);
		}

		private function updateTags( e:Event ):void {
			var a:Number;
			var b:Number;
			if( active ){
				a = (-Math.min( Math.max( holder.mouseY, -250 ), 250 ) / 150 ) * tspeed;
				b = (Math.min( Math.max( holder.mouseX, -250 ), 250 ) /150 ) * tspeed;
			} else {
				a = lasta * 0.98;
				b = lastb * 0.98;
			}
			lasta = a;
			lastb = b;
			// if a and b under threshold, skip motion calculations to free up the processor
			if( Math.abs(a) > 0.01 || Math.abs(b) > 0.01 ){
				var c:Number = 0;
				sineCosine( a, b, c );
				// bewegen van de punten
				for( var j:Number=0; j<mcList.length; j++ ) {
					// multiply positions by a x-rotation matrix
					var rx1:Number = mcList[j].cx;
					var ry1:Number = mcList[j].cy * ca + mcList[j].cz * -sa;
					var rz1:Number = mcList[j].cy * sa + mcList[j].cz * ca;
					// multiply new positions by a y-rotation matrix
					var rx2:Number = rx1 * cb + rz1 * sb;
					var ry2:Number = ry1;
					var rz2:Number = rx1 * -sb + rz1 * cb;
					// multiply new positions by a z-rotation matrix
					var rx3:Number = rx2 * cc + ry2 * -sc;
					var ry3:Number = rx2 * sc + ry2 * cc;
					var rz3:Number = rz2;
					// set arrays to new positions
					mcList[j].cx = rx3;
					mcList[j].cy = ry3;
					mcList[j].cz = rz3;
					// add perspective
					var per:Number = d / (d+rz3);
					// setmc position, scale, alpha
					mcList[j].x = rx3 * per;
					mcList[j].y = ry3 * per;
					mcList[j].scaleX = mcList[j].scaleY =  per;
					mcList[j].alpha = per/2;
				}
				depthSort();
			}
		}
		
		private function depthSort():void {
			mcList.sortOn( "cz", Array.DESCENDING | Array.NUMERIC );
			var current:Number = 0;
			for( var i:Number=0; i<mcList.length; i++ ){
				holder.setChildIndex( mcList[i], i );
				if( mcList[i].active == true ){
					current = i;
				}
			}
			holder.setChildIndex( mcList[current], mcList.length-1 );
		}
		
		/* See http://blog.massivecube.com/?p=9 */
		private function positionAll():void {		
			var phi:Number = 0;
			var theta:Number = 0;
			var max:Number = mcList.length;
			// mix up the list so not all a' live on the north pole
			mcList.sort( function(){ return Math.random()<0.5 ? 1 : -1; } );
			// distibute
			for( var i:Number=1; i<max+1; i++){
				if( distr ){
					phi = Math.acos(-1+(2*i-1)/max);
					theta = Math.sqrt(max*Math.PI)*phi;
				}else{
					phi = Math.random()*(Math.PI);
					theta = Math.random()*(2*Math.PI);
				}
				// Coordinate conversion
				mcList[i-1].cx = radius * Math.cos(theta)*Math.sin(phi);
				mcList[i-1].cy = radius * Math.sin(theta)*Math.sin(phi);
				mcList[i-1].cz = radius * Math.cos(phi);
			}
		}
		
		private function menuItemSelectHandler( e:ContextMenuEvent ):void {
			var request:URLRequest = new URLRequest( "http://www.roytanck.com" );
			navigateToURL(request);
		}
		
		private function mouseExitHandler( e:Event ):void { active = false; }
		private function mouseMoveHandler( e:MouseEvent ):void {	active = true; }
		private function resizeHandler( e:Event ):void { resizeHolder(); }
		
		private function resizeHolder():void {
			var s:Stage = this.stage;
			holder.x = s.stageWidth/2;
			holder.y = s.stageHeight/2;
			var scale:Number;
			if( s.stageWidth > s.stageHeight ){
				scale = (s.stageHeight/500);
			} else {
				scale = (s.stageWidth/500);
			}
			holder.scaleX = holder.scaleY = scale;
			// scale mousetrap too
			mousetrap_mc.width = s.stageWidth;
			mousetrap_mc.height = s.stageHeight;
		}
		
		private function sineCosine( a:Number, b:Number, c:Number ):void {
			sa = Math.sin(a * dtr);
			ca = Math.cos(a * dtr);
			sb = Math.sin(b * dtr);
			cb = Math.cos(b * dtr);
			sc = Math.sin(c * dtr);
			cc = Math.cos(c * dtr);
		}
		
		private function getNumberFromString( s:String ):Number {
			return( Number( s.match( /(\d|\.|\,)/g ).join("").split(",").join(".") ) );
		}
		
		private function getColorFromGradient( perc:Number ):Number {
			var r:Number = ( perc * ( tcolor >> 16 ) ) + ( (1-perc) * ( tcolor2 >> 16 ) );
			var g:Number = ( perc * ( (tcolor >> 8) % 256 ) ) + ( (1-perc) * ( (tcolor2 >> 8) % 256 ) );
			var b:Number = ( perc * ( tcolor % 256 ) ) + ( (1-perc) * ( tcolor2 % 256 ) );
			return( (r << 16) | (g << 8) | b );
		}
		
	}

}