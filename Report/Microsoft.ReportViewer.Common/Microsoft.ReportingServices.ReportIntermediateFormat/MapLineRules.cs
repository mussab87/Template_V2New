using Microsoft.ReportingServices.RdlExpressions.ExpressionHostObjectModel;
using Microsoft.ReportingServices.ReportIntermediateFormat.Persistence;
using Microsoft.ReportingServices.ReportProcessing;
using Microsoft.ReportingServices.ReportProcessing.OnDemandReportObjectModel;
using Microsoft.ReportingServices.ReportPublishing;
using System;
using System.Collections.Generic;

namespace Microsoft.ReportingServices.ReportIntermediateFormat
{
	[Serializable]
	internal sealed class MapLineRules : IPersistable
	{
		[NonSerialized]
		private MapLineRulesExprHost m_exprHost;

		[NonSerialized]
		private MapLineRulesExprHost m_exprHostMapMember;

		[Reference]
		private Map m_map;

		[NonSerialized]
		private static readonly Declaration m_Declaration = GetDeclaration();

		private MapSizeRule m_mapSizeRule;

		private MapColorRule m_mapColorRule;

		internal MapSizeRule MapSizeRule
		{
			get
			{
				return m_mapSizeRule;
			}
			set
			{
				m_mapSizeRule = value;
			}
		}

		internal MapColorRule MapColorRule
		{
			get
			{
				return m_mapColorRule;
			}
			set
			{
				m_mapColorRule = value;
			}
		}

		internal string OwnerName => m_map.Name;

		internal MapLineRulesExprHost ExprHost => m_exprHost;

		internal MapLineRules()
		{
		}

		internal MapLineRules(Map map)
		{
			m_map = map;
		}

		internal void Initialize(InitializationContext context)
		{
			context.ExprHostBuilder.MapLineRulesStart();
			if (m_mapSizeRule != null)
			{
				m_mapSizeRule.Initialize(context);
			}
			if (m_mapColorRule != null)
			{
				m_mapColorRule.Initialize(context);
			}
			context.ExprHostBuilder.MapLineRulesEnd();
		}

		internal void InitializeMapMember(InitializationContext context)
		{
			context.ExprHostBuilder.MapLineRulesStart();
			if (m_mapSizeRule != null)
			{
				m_mapSizeRule.InitializeMapMember(context);
			}
			if (m_mapColorRule != null)
			{
				m_mapColorRule.InitializeMapMember(context);
			}
			context.ExprHostBuilder.MapLineRulesEnd();
		}

		internal object PublishClone(AutomaticSubtotalContext context)
		{
			MapLineRules mapLineRules = (MapLineRules)MemberwiseClone();
			mapLineRules.m_map = context.CurrentMapClone;
			if (m_mapSizeRule != null)
			{
				mapLineRules.m_mapSizeRule = (MapSizeRule)m_mapSizeRule.PublishClone(context);
			}
			if (m_mapColorRule != null)
			{
				mapLineRules.m_mapColorRule = (MapColorRule)m_mapColorRule.PublishClone(context);
			}
			return mapLineRules;
		}

		internal void SetExprHost(MapLineRulesExprHost exprHost, ObjectModelImpl reportObjectModel)
		{
			Global.Tracer.Assert(exprHost != null && reportObjectModel != null, "(exprHost != null && reportObjectModel != null)");
			m_exprHost = exprHost;
			m_exprHost.SetReportObjectModel(reportObjectModel);
			if (m_mapSizeRule != null && ExprHost.MapSizeRuleHost != null)
			{
				m_mapSizeRule.SetExprHost(ExprHost.MapSizeRuleHost, reportObjectModel);
			}
			if (m_mapColorRule != null && ExprHost.MapColorRuleHost != null)
			{
				m_mapColorRule.SetExprHost(ExprHost.MapColorRuleHost, reportObjectModel);
			}
		}

		internal void SetExprHostMapMember(MapLineRulesExprHost exprHost, ObjectModelImpl reportObjectModel)
		{
			Global.Tracer.Assert(exprHost != null && reportObjectModel != null, "(exprHost != null && reportObjectModel != null)");
			m_exprHostMapMember = exprHost;
			m_exprHostMapMember.SetReportObjectModel(reportObjectModel);
			if (m_mapSizeRule != null && m_exprHostMapMember.MapSizeRuleHost != null)
			{
				m_mapSizeRule.SetExprHostMapMember(m_exprHostMapMember.MapSizeRuleHost, reportObjectModel);
			}
			if (m_mapColorRule != null && m_exprHostMapMember.MapColorRuleHost != null)
			{
				m_mapColorRule.SetExprHostMapMember(m_exprHostMapMember.MapColorRuleHost, reportObjectModel);
			}
		}

		internal static Declaration GetDeclaration()
		{
			List<MemberInfo> list = new List<MemberInfo>();
			list.Add(new MemberInfo(MemberName.MapSizeRule, Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType.MapSizeRule));
			list.Add(new MemberInfo(MemberName.MapColorRule, Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType.MapColorRule));
			list.Add(new MemberInfo(MemberName.Map, Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType.Map, Token.Reference));
			return new Declaration(Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType.MapLineRules, Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType.None, list);
		}

		public void Serialize(IntermediateFormatWriter writer)
		{
			writer.RegisterDeclaration(m_Declaration);
			while (writer.NextMember())
			{
				switch (writer.CurrentMember.MemberName)
				{
				case MemberName.Map:
					writer.WriteReference(m_map);
					break;
				case MemberName.MapSizeRule:
					writer.Write(m_mapSizeRule);
					break;
				case MemberName.MapColorRule:
					writer.Write(m_mapColorRule);
					break;
				default:
					Global.Tracer.Assert(condition: false);
					break;
				}
			}
		}

		public void Deserialize(IntermediateFormatReader reader)
		{
			reader.RegisterDeclaration(m_Declaration);
			while (reader.NextMember())
			{
				switch (reader.CurrentMember.MemberName)
				{
				case MemberName.Map:
					m_map = reader.ReadReference<Map>(this);
					break;
				case MemberName.MapSizeRule:
					m_mapSizeRule = (MapSizeRule)reader.ReadRIFObject();
					break;
				case MemberName.MapColorRule:
					m_mapColorRule = (MapColorRule)reader.ReadRIFObject();
					break;
				default:
					Global.Tracer.Assert(condition: false);
					break;
				}
			}
		}

		public void ResolveReferences(Dictionary<Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType, List<MemberReference>> memberReferencesCollection, Dictionary<int, IReferenceable> referenceableItems)
		{
			if (!memberReferencesCollection.TryGetValue(m_Declaration.ObjectType, out List<MemberReference> value))
			{
				return;
			}
			foreach (MemberReference item in value)
			{
				MemberName memberName = item.MemberName;
				if (memberName == MemberName.Map)
				{
					Global.Tracer.Assert(referenceableItems.ContainsKey(item.RefID));
					m_map = (Map)referenceableItems[item.RefID];
				}
				else
				{
					Global.Tracer.Assert(condition: false);
				}
			}
		}

		public Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType GetObjectType()
		{
			return Microsoft.ReportingServices.ReportIntermediateFormat.Persistence.ObjectType.MapLineRules;
		}
	}
}
