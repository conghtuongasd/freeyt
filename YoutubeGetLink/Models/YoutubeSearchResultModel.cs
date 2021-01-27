using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoutubeGetLink.Models
{
    public class Thumbnail2
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }

    }

    public class Thumbnail
    {
        public List<Thumbnail2> thumbnails { get; set; }

    }

    public class Run
    {
        public string text { get; set; }

    }

    public class AccessibilityData
    {
        public string label { get; set; }

    }

    public class Accessibility
    {
        public AccessibilityData accessibilityData { get; set; }

    }

    public class Title
    {
        public List<Run> runs { get; set; }
        public Accessibility accessibility { get; set; }

    }

    public class Run2
    {
        public string text { get; set; }
        public bool bold { get; set; }

    }

    public class DescriptionSnippet
    {
        public List<Run2> runs { get; set; }

    }

    public class PublishedTimeText
    {
        public string simpleText { get; set; }

    }

    public class AccessibilityData2
    {
        public string label { get; set; }

    }

    public class Accessibility2
    {
        public AccessibilityData2 accessibilityData { get; set; }

    }

    public class LengthText
    {
        public Accessibility2 accessibility { get; set; }
        public string simpleText { get; set; }

    }

    public class ViewCountText
    {
        public string simpleText { get; set; }

    }

    public class Run3
    {
        public string text { get; set; }

    }

    public class OwnerText
    {
        public List<Run3> runs { get; set; }

    }

    public class ShortViewCountText
    {
        public string simpleText { get; set; }

    }

    public class VideoRenderer
    {
        public string videoId { get; set; }
        public Thumbnail thumbnail { get; set; }
        public Title title { get; set; }
        public DescriptionSnippet descriptionSnippet { get; set; }
        public PublishedTimeText publishedTimeText { get; set; }
        public LengthText lengthText { get; set; }
        public ViewCountText viewCountText { get; set; }
        public OwnerText ownerText { get; set; }
        public ShortViewCountText shortViewCountText { get; set; }

    }

    public class Content2
    {
        public VideoRenderer videoRenderer { get; set; }

    }

    public class ItemSectionRenderer
    {
        public List<Content2> contents { get; set; }

    }

    public class ContinuationCommand
    {
        public string token { get; set; }
        public string request { get; set; }
    }

    public class ContinuationEndpoint
    {
        public ContinuationCommand continuationCommand { get; set; }
    }

    public class ContinuationItemRenderer
    {
        public ContinuationEndpoint continuationEndpoint { get; set; }
    }

    public class Content
    {
        public ItemSectionRenderer itemSectionRenderer { get; set; }
        public ContinuationItemRenderer continuationItemRenderer { get; set; }
        public SectionListRenderer sectionListRenderer { get; set; }
    }

    public class SectionListRenderer
    {
        public List<Content> contents { get; set; }
    }

    public class PrimaryContents
    {
        public SectionListRenderer sectionListRenderer { get; set; }

    }

    public class TwoColumnSearchResultsRenderer
    {
        public PrimaryContents primaryContents { get; set; }

    }

    public class Contents
    {
        public TwoColumnSearchResultsRenderer twoColumnSearchResultsRenderer { get; set; }
        public dynamic twoColumnBrowseResultsRenderer { get; set; }
    }

    public class ContinuationItem
    {
        public ItemSectionRenderer itemSectionRenderer { get; set; }
        public ContinuationItemRenderer continuationItemRenderer { get; set; }
    }

    public class AppendContinuationItemsAction
    {
        public List<ContinuationItem> continuationItems { get; set; }
    }

    public class OnResponseReceivedCommand
    {
        public AppendContinuationItemsAction appendContinuationItemsAction { get; set; }
    }

    public class TabRenderer
    {
        public Content content { get; set; }
    }

    public class Tab
    {
        public TabRenderer tabRenderer { get; set; }
    }

    public class TwoColumnBrowseResultsRenderer
    {
        public List<Tab> tabs { get; set; }
    }

    public class YoutubeSearchResultModel
    {
        public Contents contents { get; set; }
        public List<OnResponseReceivedCommand> onResponseReceivedCommands { get; set; }
    }
}
