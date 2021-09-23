﻿#region Apache License Version 2.0
/*----------------------------------------------------------------

Copyright 2021 Jeffrey Su & Suzhou Senparc Network Technology Co.,Ltd.

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
except in compliance with the License. You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software distributed under the
License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
either express or implied. See the License for the specific language governing permissions
and limitations under the License.

Detail: https://github.com/JeffreySu/WeiXinMPSDK/blob/master/license.md

----------------------------------------------------------------*/
#endregion Apache License Version 2.0

/*----------------------------------------------------------------
    Copyright (C) 2021 Senparc
  
    文件名：MarketingApis.cs
    文件功能描述：微信支付V3营销工具接口
    
    
    创建标识：Senparc - 20210821
    
----------------------------------------------------------------*/

using Senparc.CO2NET.Extensions;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.Trace;
using Senparc.Weixin.Entities;
using Senparc.Weixin.TenPayV3.Apis.Entities;
using Senparc.Weixin.TenPayV3.Apis.Marketing;
using Senparc.Weixin.TenPayV3.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.TenPayV3.Apis
{
    /// <summary>
    /// 微信支付V3营销工具接口
    /// https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter3_1_1.shtml 下的【营销工具】所有接口
    /// </summary>
    public partial class MarketingApis
    {

        private ISenparcWeixinSettingForTenpayV3 _tenpayV3Setting;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="senparcWeixinSettingForTenpayV3"></param>
        public MarketingApis(ISenparcWeixinSettingForTenpayV3 senparcWeixinSettingForTenpayV3 = null)
        {

            _tenpayV3Setting = senparcWeixinSettingForTenpayV3 ?? Senparc.Weixin.Config.SenparcWeixinSetting.TenpayV3Setting;
        }

        /// <summary>
        /// 返回可用的微信支付地址（自动判断是否使用沙箱）
        /// </summary>
        /// <param name="urlFormat">如：<code>https://api.mch.weixin.qq.com/{0}pay/unifiedorder</code></param>
        /// <returns></returns>
        // TODO: 重复使用
        private static string ReurnPayApiUrl(string urlFormat)
        {
            return string.Format(urlFormat, Senparc.Weixin.Config.UseSandBoxPay ? "sandboxnew/" : "");
        }

        #region 商家券接口

        /// <summary>
        /// 创建商家券批次接口
        /// <para>商户可以通过该接口创建商家券。微信支付生成商家券批次后并返回商家券批次号给到商户，商户可调用发券接口发放该批次商家券。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_1.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<CreateBusifavorStockReturnJson> CreateBusifavorStockRequestDataAsync(CreateBusifavorStockRequestData data, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl("https://api.mch.weixin.qq.com/{0}v3/marketing/busifavor/stocks");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<CreateBusifavorStockReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 查询商家券批次详情接口
        /// <para>商户可通过该接口查询已创建的商家券批次详情信息。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_2.shtml </para>
        /// </summary>
        /// <param name="stock_id">微信为每个商家券批次分配的唯一ID</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<QueryBusifavorStockReturnJson> QueryBusifavorStockAsync(string stock_id, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/stocks/{stock_id}");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<QueryBusifavorStockReturnJson>(url, null, timeOut, ApiRequestMethod.GET);
        }

        /// <summary>
        /// 核销商家券用户券详情接口
        /// <para>在用户满足优惠门槛后，商户可通过该接口核销用户微信卡包中具体某一张商家券。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_3.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<UseBusifavorCouponReturnJson> UseBusifavorCouponAsync(UseBusifavorCouponRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl("https://api.mch.weixin.qq.com/{0}v3/marketing/busifavor/coupons/use");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<UseBusifavorCouponReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 根据过滤条件查询商家券用户券接口
        /// <para>商户自定义筛选条件（如创建商户号、归属商户号、发放商户号等），查询指定微信用户卡包中满足对应条件的所有商家券信息。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_4.shtml </para>
        /// </summary>
        /// <param name="openid">Openid信息，用户在appid下的唯一标识</param>
        /// <param name="appid">支持传入与当前调用接口商户号有绑定关系的appid。支持小程序appid与公众号appid</param>
        /// <param name="stock_id">微信为每个商家券批次分配的唯一ID，是否指定批次号查询</param>
        /// <param name="coupon_state">券状态 枚举值：SENDED：可用 USED：已核销 EXPIRED：已过期,可为null</param>
        /// <param name="creator_merchant">批次创建方商户号,可为null</param>
        /// <param name="belong_merchant">批次归属商户号,可为null</param>
        /// <param name="sender_merchant">批次发放商户号,可为null</param>
        /// <param name="offset">分页页码 默认值0</param>
        /// <param name="limit">分页大小 默认值20</param>
        /// <param name="timeOut">超时时间，单位为ms</param>
        /// <returns></returns>
        public async Task<QueryBusifavorCouponsReturnJson> QueryBusifavorCouponsAsync(string openid, string appid, string stock_id, string coupon_state, string creator_merchant, string belong_merchant, string sender_merchant, int offset = 0, int limit = 20, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/users/{openid}/coupons?appid={appid}&offset={offset}&limit={limit}");
      
            url += stock_id is not null ? $"&stock_id={stock_id}" : "";
            url += coupon_state is not null ? $"&coupon_state={coupon_state}" : "";
            url += creator_merchant is not null ? $"&creator_merchant={creator_merchant}" : "";
            url += belong_merchant is not null ? $"&belong_merchant={belong_merchant}" : "";
            url += belong_merchant is not null ? $"&belong_merchant={belong_merchant}" : "";
            url += sender_merchant is not null ? $"&sender_merchant={sender_merchant}" : "";

            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<QueryBusifavorCouponsReturnJson>(url, null, timeOut, ApiRequestMethod.GET);
        }

        /// <summary>
        /// 查询用户单张商家券详情接口
        /// <para>商户可通过该接口查询微信用户卡包中某一张商家券的详情信息</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_5.shtml </para>
        /// </summary>
        /// <param name="coupon_code">券的唯一标识</param>
        /// <param name="appid">支持传入与当前调用接口商户号有绑定关系的appid。支持小程序appid与公众号appid</param>
        /// <param name="openid"> Openid信息，用户在appid下的唯一标识</param>
        /// <param name="timeOut">超时时间，单位为ms</param>
        /// <returns></returns>
        public async Task<QueryBusifavorCouponReturnJson> QueryBusifavorCouponAsync(string coupon_code, string appid, string openid, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/users/{openid}/coupons/{coupon_code}/appids/{appid}");

            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<QueryBusifavorCouponReturnJson>(url, null, timeOut, ApiRequestMethod.GET);
        }

        /// <summary>
        /// 上传预存code接口
        /// <para>商家券的Code码可由微信后台随机分配，同时支持商户自定义。如商家已有自己的优惠券系统，可直接使用自定义模式。即商家预先向微信支付上传券Code，当券在发放时，微信支付自动从已导入的Code中随机取值（不能指定），派发给用户。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_6.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<SetBusifavorCouponCodesReturnJson> SetBusifavorCouponCodesAsync(SetBusifavorCouponCodesRequestData data, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/stocks/{data.stock_id}/couponcodes");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<SetBusifavorCouponCodesReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 设置商家券事件通知地址接口
        /// <para>用于设置接收商家券相关事件通知的URL，可接收商家券相关的事件通知、包括发放通知等。需要设置接收通知的URL，并在商户平台开通营销事件推送的能力，即可接收到相关通知。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_7.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<SetBusifavorSetNotifyUrlReturnJson> SetBusifavorSetNotifyUrlAsync(SetBusifavorSetNotifyUrlRequestData data, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/callbacks");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<SetBusifavorSetNotifyUrlReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 查询商家券事件通知地址API
        /// <para>通过调用此接口可查询设置的通知URL</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_8.shtml </para>
        /// </summary>
        /// <param name="mchid">微信支付商户的商户号，由微信支付生成并下发，不填默认查询调用方商户的通知URL</param>
        /// <param name="timeOut">超时时间，单位为ms</param>
        /// <returns></returns>
        public async Task<QueryBusifavorNotifyUrlReturnJson> QueryBusifavorNotifyUrlAsync(string mchid, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/callbacks?mchid={mchid}");

            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<QueryBusifavorNotifyUrlReturnJson>(url, null, timeOut, ApiRequestMethod.GET);
        }

        /// <summary>
        /// 关联订单信息接口
        /// <para>将有效态（未核销）的商家券与订单信息关联，用于后续参与摇奖&返佣激励等操作的统计。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_9.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<AssociateBusifavorReturnJson> AssociateBusifavorAsync(AssociateBusifavorRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/coupons/associate");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<AssociateBusifavorReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 关联订单信息接口
        /// <para>将有效态（未核销）的商家券与订单信息关联，用于后续参与摇奖&返佣激励等操作的统计。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_9.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<DisassociateBusifavorReturnJson> DisassociateBusifavorAsync(DisassociateBusifavorRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/coupons/disassociate");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<DisassociateBusifavorReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 修改批次预算接口
        /// <para>商户可以通过该接口修改批次单天发放上限数量或者批次最大发放数量</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_11.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<ModifyBusifavorStockBudgetReturnJson> ModifyBusifavorStockBudgetAsync(ModifyBusifavorStockBudgetRequestData data, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/stocks/{data.stock_id}/budget");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<ModifyBusifavorStockBudgetReturnJson>(url, data, timeOut, ApiRequestMethod.PATCH);
        }


        /// <summary>
        /// 修改商家券基本信息接口
        /// <para>商户可以通过该接口修改商家券基本信息</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_12.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<ReturnJsonBase> ModifyBusifavorStockInformationAsync(ModifyBusifavorStockInformationRequestData data, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/stocks/{data.stock_id}");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<ReturnJsonBase>(url, data, timeOut, ApiRequestMethod.PATCH);
        }

        /// <summary>
        /// 申请退券接口
        /// <para>商户可以在退款之后调用退券api，调用了该接口后，券在用户卡包内正常展示，用户可在有效期内正常使用该优惠券。</para>
        /// <para>若券过期状态下退款，可不用调用该退券api。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_13.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<ReturnBusifavorCouponReturnJson> ReturnBusifavorCouponAsync(ReturnBusifavorCouponRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/coupons/return");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<ReturnBusifavorCouponReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 使券失效接口
        /// <para>商户可通过该接口将单张领取后未核销的券进行失效处理，券失效后无法再被核销</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_14.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<DeactivateBusifavorCouponReturnJson> DeactivateBusifavorCouponAsync(DeactivateBusifavorCouponRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/coupons/deactivate");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<DeactivateBusifavorCouponReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 营销补差付款接口
        /// <para>该API主要用于商户营销补贴场景，支持by单张券进行不同商户账户间的资金补差，从而提升财务结算、资金利用效率。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_16.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<PayBusifavorReceiptsReturnJson> PayBusifavorReceiptsAsync(PayBusifavorReceiptsRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/subsidy/pay-receipts");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<PayBusifavorReceiptsReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 查询营销补差付款单详情API
        /// <para>查询商家券营销补差付款单详情</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_2_18.shtml </para>
        /// </summary>
        /// <param name="subsidy_receipt_id">补差付款唯一单号，由微信支付生成，仅在补差付款成功后有返回</param>
        /// <param name="timeOut">超时时间，单位为ms</param>
        /// <returns></returns>
        public async Task<QueryBusifavorPayReceiptsReturnJson> QueryBusifavorPayReceiptsAsync(string subsidy_receipt_id, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/subsidy/pay-receipts/{subsidy_receipt_id}");

            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<QueryBusifavorPayReceiptsReturnJson>(url, null, timeOut, ApiRequestMethod.GET);
        }

        #endregion

        #region 委托营销接口

        /// <summary>
        /// 建立合作关系接口
        /// <para>该接口主要为商户提供营销资源的授权能力，可授权给其他商户或小程序，方便商户间的互利合作。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_5_1.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<BuildPartnershipsReturnJson> BuildPartnershipsAsync(BuildPartnershipsRequestData data, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl("https://api.mch.weixin.qq.com/{0}v3/marketing/partnerships/build");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<BuildPartnershipsReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 终止合作关系接口
        /// <para>该接口主要为商户提供营销资源的终止授权能力，便于商户管理运营现存的合作关系。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_5_2.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<TerminatePartnershipsReturnJson> TerminatePartnershipsAsync(TerminatePartnershipsRequestData data, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl("https://api.mch.weixin.qq.com/{0}v3/marketing/partnerships/terminate");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<TerminatePartnershipsReturnJson>(url, data, timeOut);
        }


        /// <summary>
        /// 查询合作关系接口
        /// <para>该接口主要为商户提供合作关系列表的查询能力。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_5_3.shtml </para>
        /// </summary>
        /// <param name="data">询合作关系需要的Data数据</param>
        /// <param name="limit">分页大小<para>query分页大小，最大50。不传默认为20。</para><para>示例值：5</para><para>可为null</para></param>
        /// <param name="offset">分页页码<para>query分页页码，页码从0开始。</para><para>示例值：10</para><para>可为null</para></param>
        /// <param name="timeOut">超时时间，单位为ms</param>
        /// <returns></returns>
        public async Task<QueryPartnershipsReturnJson> QueryPartnershipsAsync(TerminatePartnershipsRequestData data, ulong limit, ulong offset, int timeOut = Config.TIME_OUT)
        {
            // TODO: 此处序列化Json需测试时候可以换行问题
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/partnerships?authorized_data={data.authorized_data.ToJson()}&partner={data.partner.ToJson()}&offset={offset}&limit={limit}".UrlEncode());
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<QueryPartnershipsReturnJson>(url, null, timeOut, ApiRequestMethod.GET);
        }

        #endregion

        #region 消费卡接口

        /// <summary>
        /// 发放消费卡接口
        /// <para>商户通过调用本接口向用户发放消费卡，用户领到卡的同时会领取到一批代金券，消费卡会自动放入卡包中。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_6_1.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<SendCardReturnJson> SendCardAsync(SendCardRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/busifavor/coupons/{data.card_id}/send");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<SendCardReturnJson>(url, data, timeOut);
        }

        #endregion

        #region 支付有礼接口

        /// <summary>
        /// 创建全场满额送活动接口
        /// <para>商户可以创建满额送活动，用户支付后送全场券，提升交易额。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_7_2.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<CreateUniqueThresholdActivityReturnJson> CreateUniqueThresholdActivityAsync(CreateUniqueThresholdActivityRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/paygiftactivity/unique-threshold-activity");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<CreateUniqueThresholdActivityReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 查询活动详情接口
        /// <para>商户创建活动后，可以通过该接口查询支付有礼的活动详情，用于管理活动。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_7_4.shtml </para>
        /// </summary>
        /// <param name="activity_id">活动id</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<QueryPaygiftActivityReturnJson> QueryPaygiftActivityAsync(string activity_id, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/paygiftactivity/activities/{activity_id}");

            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<QueryPaygiftActivityReturnJson>(url, null, timeOut, ApiRequestMethod.GET);
        }

        /// <summary>
        /// 查询活动发券商户号接口
        /// <para>商户创建活动后，可以通过该接口查询支付有礼的发券商户号，用于管理活动。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_7_5.shtml </para>
        /// </summary>
        /// <param name="activity_id">活动id</param>
        /// <param name="limit">分页大小，最大50。不传默认为20。 默认值：10</param>
        /// <param name="offset">分页页码，页码从0开始。 默认值：0</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<QueryPaygiftActivityMerchantsReturnJson> QueryPaygiftActivityMerchantsAsync(string activity_id, ulong limit = 10, ulong offset = 0, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/paygiftactivity/activities/{activity_id}/merchants?offset={offset}&limit={limit}");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<QueryPaygiftActivityMerchantsReturnJson>(url, null, timeOut, ApiRequestMethod.GET);
        }

        /// <summary>
        /// 查询活动指定商品列表接口
        /// <para>商户创建活动后，可以通过该接口查询支付有礼的活动指定商品，用于管理活动。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_7_6.shtml </para>
        /// </summary>
        /// <param name="activity_id">活动id</param>
        /// <param name="limit">分页大小，最大50。不传默认为20。 默认值：10</param>
        /// <param name="offset">分页页码，页码从0开始。 默认值：0</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<QueryPaygiftActivityGoodsReturnJson> QueryPaygiftActivityGoodsAsync(string activity_id, ulong limit = 10, ulong offset = 0, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/paygiftactivity/activities/{activity_id}/goods?offset={offset}&limit={limit}");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<QueryPaygiftActivityGoodsReturnJson>(url, null, timeOut, ApiRequestMethod.GET);
        }

        /// <summary>
        /// 终止支付有礼活动接口
        /// <para>商户可以创建满额送活动，用户支付后送全场券，提升交易额。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_7_7.shtml </para>
        /// </summary>
        /// <param name="activity_id">支付有礼活动id</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<TerminatePaygiftActivityReturnJson> TerminatePaygiftActivityAsync(string activity_id, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/paygiftactivity/activities/{activity_id}/terminate");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            // TODO: 此处应该注意检查post方法body为null时候有问题 文档确实body没有传任何数据
            return await tenPayApiRequest.RequestAsync<TerminatePaygiftActivityReturnJson>(url, null, timeOut);
        }

        /// <summary>
        /// 删除活动发券商户号接口
        /// <para>商户创建活动后，可以通过该接口增加支付有礼的发券商户号，用于管理活动。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_7_8.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<AddPaygiftActivityMerchantsReturnJson> AddPaygiftActivityMerchantsAsync(AddPaygiftActivityMerchantsRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/paygiftactivity/activities/{data.activity_id}/merchants/add");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<AddPaygiftActivityMerchantsReturnJson>(url, data, timeOut);
        }

        /// <summary>
        /// 获取支付有礼活动列表接口
        /// <para>商户根据一定过滤条件，查询已创建的支付有礼活动。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_7_9.shtml </para>
        /// </summary>
        /// <param name="activity_name">活动名称，支持模糊搜索</param>
        /// <param name="activity_status">活动状态，枚举值：ACT_STATUS_UNKNOWN：状态未知 CREATE_ACT_STATUS：已创建 ONGOING_ACT_STATUS：运行中 TERMINATE_ACT_STATUS：已终止 STOP_ACT_STATUS：已暂停 OVER_TIME_ACT_STATUS：已过期 CREATE_ACT_FAILED：创建活动失败</param>
        /// <param name="award_type"> 奖品类型，暂时只支持商家券 枚举值: BUSIFAVOR：商家券</param>
        /// <param name="limit">分页大小，最大50。不传默认为20。 默认值：10</param>
        /// <param name="offset">分页页码，页码从0开始。 默认值：0</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<QueryPaygiftActivitiesReturnJson> QueryPaygiftActivitiesAsync(string activity_name, string activity_status, string award_type, int limit = 10, int offset = 0, int timeOut = Config.TIME_OUT)
        {

            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/paygiftactivity/activities?offset={offset}&limit={limit}");

            url += activity_name is not null ? $"&activity_name={activity_name}" : "";
            url += activity_status is not null ? $"&activity_status={activity_status}" : "";
            url += award_type is not null ? $"&award_type={award_type}" : "";

            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<QueryPaygiftActivitiesReturnJson>(url, null, timeOut, ApiRequestMethod.GET);
        }

        /// <summary>
        /// 新增活动发券商户号接口
        /// <para>商户创建活动后，可以通过该接口删除支付有礼的发券商户号，用于管理活动。</para>
        /// <para>更多详细请参考 https://pay.weixin.qq.com/wiki/doc/apiv3/apis/chapter9_7_10.shtml </para>
        /// </summary>
        /// <param name="data">微信支付需要POST的Data数据</param>
        /// <param name="timeOut">超时时间，单位为ms </param>
        /// <returns></returns>
        public async Task<DeletePaygiftActivitiyMerchantsReturnJson> DeletePaygiftActivitiyMerchantsAsync(DeletePaygiftActivitiyMerchantsRequestData data, int timeOut = Config.TIME_OUT)
        {
            var url = ReurnPayApiUrl($"https://api.mch.weixin.qq.com/{{0}}v3/marketing/paygiftactivity/activities/{data.activity_id}/merchants/delete");
            TenPayApiRequest tenPayApiRequest = new(_tenpayV3Setting);
            return await tenPayApiRequest.RequestAsync<DeletePaygiftActivitiyMerchantsReturnJson>(url, data, timeOut);
        }

        #endregion

        // TODO: 图片上传接口
    }
}
