using Microsoft.ReportingServices.Diagnostics;
using Microsoft.ReportingServices.HtmlRendering;
using Microsoft.ReportingServices.Interfaces;
using Microsoft.ReportingServices.OnDemandReportRendering;
using Microsoft.ReportingServices.Rendering.SPBProcessing;
using Microsoft.ReportingServices.ReportProcessing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web.UI;

namespace Microsoft.ReportingServices.Rendering.HtmlRenderer
{
	class Html40RenderingExtension : RenderingExtensionBase
	{
		public override string LocalizedName => RenderRes.HTML40LocalizedName;

		protected override bool InternalRender(Microsoft.ReportingServices.OnDemandReportRendering.Report report, NameValueCollection reportServerParameters, NameValueCollection deviceInfo, NameValueCollection clientCapabilities, ref Hashtable renderProperties, CreateAndRegisterStream createAndRegisterStream)
		{
			HtmlTextWriter htmlTextWriter = null;
			ServerRenderer serverRenderer = null;
			try
			{
				htmlTextWriter = HtmlWriterFactory.CreateWriter(report.Name, "text/html", createAndRegisterStream, StreamOper.CreateAndRegister);
				DeviceInfo deviceInfo2 = null;
				try
				{
					deviceInfo2 = new ServerDeviceInfo();
					deviceInfo2.ParseDeviceInfo(deviceInfo, clientCapabilities);
				}
				catch (ArgumentOutOfRangeException innerException)
				{
					throw new ReportRenderingException(RenderRes.rrInvalidDeviceInfo, innerException);
				}
				bool onlyVisibleStyles = deviceInfo2.OnlyVisibleStyles;
				int totalPages = 0;
				if (renderProperties != null)
				{
					object obj = renderProperties["ClientPaginationMode"];
					if (obj != null)
					{
						PaginationMode paginationMode = (PaginationMode)obj;
						if (paginationMode == PaginationMode.TotalPages)
						{
							object obj2 = renderProperties["PreviousTotalPages"];
							if (obj2 != null && obj2 is int)
							{
								totalPages = (int)obj2;
							}
						}
					}
				}
				if (deviceInfo2.BookmarkId != null)
				{
					string uniqueName = null;
					SPBInteractivityProcessing sPBInteractivityProcessing = new SPBInteractivityProcessing();
					int section = sPBInteractivityProcessing.ProcessBookmarkNavigationEvent(report, totalPages, deviceInfo2.BookmarkId, out uniqueName);
					if (uniqueName != null)
					{
						deviceInfo2.Section = section;
						deviceInfo2.NavigationId = uniqueName;
					}
				}
				try
				{
					serverRenderer = CreateRenderer(report, reportServerParameters, deviceInfo2, deviceInfo, clientCapabilities, createAndRegisterStream, ref renderProperties, totalPages);
				}
				catch (InvalidSectionException innerException2)
				{
					throw new ReportRenderingException(innerException2);
				}
				serverRenderer.Render(htmlTextWriter);
				serverRenderer.UpdateRenderProperties(ref renderProperties);
				return false;
			}
			finally
			{
				serverRenderer?.Dispose();
				htmlTextWriter?.Flush();
			}
		}

		internal virtual ServerRenderer CreateRenderer(Microsoft.ReportingServices.OnDemandReportRendering.Report report, NameValueCollection reportServerParams, DeviceInfo deviceInfo, NameValueCollection rawDeviceInfo, NameValueCollection browserCaps, CreateAndRegisterStream createAndRegisterStreamCallback, ref Hashtable renderProperties, int totalPages)
		{
			Dictionary<string, string> globalBookmarks = new Dictionary<string, string>();
			if (!deviceInfo.HasActionScript && (deviceInfo.Section == 0 || !deviceInfo.AllowScript))
			{
				globalBookmarks = Microsoft.ReportingServices.Rendering.SPBProcessing.SPBProcessing.CollectBookmarks(report, totalPages);
			}
			Microsoft.ReportingServices.Rendering.SPBProcessing.SPBProcessing spbProcessing = new Microsoft.ReportingServices.Rendering.SPBProcessing.SPBProcessing(report, createAndRegisterStreamCallback, registerEvents: true, ref renderProperties);
			SecondaryStreams secondaryStreams = SecondaryStreams.Embedded;
			ServerRenderer serverRenderer = new ServerRenderer(new ROMReport(report), spbProcessing, reportServerParams, deviceInfo, rawDeviceInfo, browserCaps, createAndRegisterStreamCallback, secondaryStreams);
			serverRenderer.InitializeReport();
			SetParameters(serverRenderer, deviceInfo, report);
			serverRenderer.GlobalBookmarks = globalBookmarks;
			return serverRenderer;
		}

		internal void SetParameters(ServerRenderer sr, DeviceInfo deviceInfo, Microsoft.ReportingServices.OnDemandReportRendering.Report report)
		{
			if (!deviceInfo.HTMLFragment && report.Parameters != null)
			{
				sr.Parameters = report.Parameters;
			}
		}

		protected override bool InternalRenderStream(string streamName, Microsoft.ReportingServices.OnDemandReportRendering.Report report, NameValueCollection reportServerParameters, NameValueCollection deviceInfo, NameValueCollection clientCapabilities, ref Hashtable renderProperties, CreateAndRegisterStream createAndRegisterStream)
		{
			if (streamName == null || streamName.Length <= 0)
			{
				return false;
			}
			if (Microsoft.ReportingServices.Rendering.SPBProcessing.SPBProcessing.RenderSecondaryStream(report, createAndRegisterStream, streamName))
			{
				return true;
			}
			char c = '_';
			char[] separator = new char[1]
			{
				c
			};
			string[] array = streamName.Split(separator);
			if (array.Length < 2)
			{
				return false;
			}
			string text = report.Name + c + "style";
			if (streamName.StartsWith(text, StringComparison.Ordinal))
			{
				DeviceInfo deviceInfo2 = null;
				try
				{
					deviceInfo2 = new ServerDeviceInfo();
					deviceInfo2.ParseDeviceInfo(deviceInfo, clientCapabilities);
				}
				catch (ArgumentOutOfRangeException innerException)
				{
					throw new ReportRenderingException(RenderRes.rrInvalidDeviceInfo, innerException);
				}
				if (streamName.Length > text.Length && deviceInfo2.Section == 0)
				{
					int result = 0;
					string s = streamName.Substring(text.Length + 1);
					if (int.TryParse(s, out result))
					{
						deviceInfo2.Section = result;
					}
				}
				if (!deviceInfo2.OnlyVisibleStyles || deviceInfo2.Section == 0)
				{
					Stream stream = createAndRegisterStream(streamName, "css", Encoding.UTF8, "text/css", willSeek: false, StreamOper.CreateAndRegister);
					HTMLStyleRenderer hTMLStyleRenderer = new HTMLStyleRenderer(report, createAndRegisterStream, deviceInfo2, null);
					hTMLStyleRenderer.Render(stream);
					stream.Flush();
				}
				else
				{
					ServerRenderer serverRenderer = null;
					try
					{
						serverRenderer = CreateRenderer(report, reportServerParameters, deviceInfo2, deviceInfo, clientCapabilities, createAndRegisterStream, ref renderProperties, 0);
						serverRenderer.RenderStylesOnly(streamName);
						serverRenderer.UpdateRenderProperties(ref renderProperties);
					}
					catch (InvalidSectionException innerException2)
					{
						throw new ReportRenderingException(innerException2);
					}
					finally
					{
						serverRenderer?.Dispose();
					}
				}
				return true;
			}
			return false;
		}
	}
}
