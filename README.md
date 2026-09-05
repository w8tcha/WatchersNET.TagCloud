# WatchersNET.TagCloud
This DNN (formerly DotNetNuke) Module displays a Tag as an HTML5 Canvas 3D Cloud, WordCloud ("Wordle" inspired Tag Cloud), or as Plain HTML with Skinning as an list of hyper links in varying styles depending on a weight (Like a Standard Web 2.0 Tag Cloud). 

The Tags are generated from...
* An List of Custom (Localized) Tags you can define
* From the Dnn Taxonomy Terms
* Show Tags from Ventrian NewsArticles Module
* Show Tags from Ventrian SimpleGallery Module
* Show Tags from ActiveForums Module
* Show Tags from Core DotNetNuke Blog Module (You can select one or more Blogs, or all Blogs of the Portal as Source)

## Features
### 3D Cloud
By default the Tag Cloud is rendered as a plain HTML 5 3D Cloud, WordCloud ("Wordle" inspired Tag Cloud) or as Plain HTML with Skinning. You can customize the 3D Cloud, you can set... 
* The Fore Color 
* Highlight Color 
* Background Color or Transparent
* Speed 
* Width and Height
* Set Text Font of Canvas Tags
* Set Weight Method to use for displaying tag weights inside the Canvas Tag Cloud. Should be one of size, colour or both.


### Appearance (Html Skins)
This Module is shipped with 15 skins. The skins can be easily switched in the Module Settings. Users can easily create own skins because the semantic markup provides easy and flexible styling using CSS.


### Filtering
The Module allows an easy way of organizing the tags, by setting a couple of properties. This way the user can choose which tags and in what order they will appear in the cloud. The items in the TagCloud can be filtered by setting either of the following properties: 
* Minimum Tag Weight - You can define the Minimum Occurrence of the Word to show as Tag
* Minimum Tag Weight - Maximum Number of Items Shows in the TagCloud
* Sort Tags - Option to sort the Tags alphabetically or based on their weight, in ascending or descending order, or random.
* Render Tag Item Weight - Enabling this property renders a number next to each tag signifying its weight.


### Sorting
By default the tags are sorted by Alphabet Ascending. The user can choose to sort them randomly, or alphabetically or based on their weight, in ascending or descending order.  Possible options to sort the items in the cloud are: Not Sorted, Alphabetic Ascending, Alphabetic Descending, Weighted Ascending and Weighted Descending. 


### Excluding of Words
Common Words can be easily excluded based on the Current Language. Custom Exclude Words can be also defined, you can Choose how the Exclude Word Matches the Tag either Equals, Contains, Starts With or Ends Width the Tag.

## Basic Settings
You can define some Basic Settings such as...
* Define a Tag Separator - Define an Separator between each Tag, for example an '|'.
* Option to Render Html Tag Cloud as Unordered List (UL) - Or as simple List of Hyper Links
* Cache Tags - If the Tags are Cached, they don't need generate every Time when the Page is loaded this can increase Page Load Time
* Tags with Link - The Tag can contain a Link to the Portal Search with current word.
* Check Url Visibility - Check Tag Urls if the current User can see Page of the Tag or Tag Link, if Not The Tag is not shown.

### Easy Color Settings Selection
The Color Settings can be easily changed with an Color Picker

### IPortable Support 
Easy Import and Export of all Module Settings

*Screenshots*
![wordle" - Word Cloud](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/dnnTagCloudWordle.png?raw=true)

![The 3D HTML Tag Cloud](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/TagCloudCanvas.jpg?raw=true)

![Sliding-Green Skin](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/Sliding-Green.png?raw=true)

![Rotated Skin](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/TagCloudSkinRotated.jpg?raw=true)

![TagTastic Skin](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/TagCloud-TagTastic-Skin.jpg?raw=true)

![RoundedButton Skin](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/TagCloud-RoundedButton-Skin.jpg?raw=true)

![Html Skin Orange](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/TagCloudSkinOrange.jpg?raw=true)

![Html Skin Purple](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/TagCloudSkinPurple.jpg?raw=true)

![Basic Settings](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/TagCloudBasicSettings.jpg?raw=true)

![Source Settings](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/TagCloudSourceSettings.jpg?raw=true)

![Exclude Settings](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/TagCloudExludes.jpg?raw=true)

![Custom Tags Export Dialog](https://github.com/w8tcha/WatchersNET.TagCloud/blob/master/screenshots/TagCloudExportDialog.jpg?raw=true)
