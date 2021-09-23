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
  
    文件名：TenpayDateTime.cs
    文件功能描述：微信支付 V3 日期时间类
    
    
    创建标识：Senparc - 20210804
    
----------------------------------------------------------------*/

using Senparc.Weixin.TenPayV3.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Weixin.TenPayV3.Entities
{
    /// <summary>
    /// 微信支付请求接口中的提起格式，可以通过 .ToString() 方法生成标准微信支付格式的字符串
    /// </summary>
    //[Serializable]
    public class TenpayDateTime //: ISerializable
    {
        private bool _keepMillionSeconds ;

        public TenpayDateTime(DateTime dateTime, bool keepMillionSeconds = true)
        {
            DateTime = dateTime;
            _keepMillionSeconds = keepMillionSeconds;
        }
        public DateTime DateTime { get; }

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    info.AddValue("DateTime", context.Context, typeof(DateTime));
        //    info.SetType(typeof(DateTime));
        //}

        public override string ToString()
        {
            return DateTime.ToTenPayDateTime(_keepMillionSeconds);
        }
    }
}
