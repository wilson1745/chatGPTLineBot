using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace isRock.Template
{
    public class LineWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        [Route("api/LineBotWebHook")]
        [HttpPost]
        public IActionResult POST()
        {
            var AdminUserId = "U4fb865f6dbe9b0ed361b915695022e49";

            try
            {
                //設定ChannelAccessToken
                this.ChannelAccessToken = "6Gi6Wrb66sqTZodRKegVRzJzTp8LDkTMdVC4znBwoaGaC1SmCvLrKPbCFU+Z1LXM2feIMugYuskA/AD4Sl7DkEWkveexAQQuAYlkQxiEU9q/vvzzzehEoqY3ZnhEtK60O1u/3pACPh5sTe+E//BzzgdB04t89/1O/w1cDnyilFU=";

                //配合Line Verify
                if(ReceivedMessage.events == null || ReceivedMessage.events.Count() <= 0 ||
                    ReceivedMessage.events.FirstOrDefault().replyToken == "00000000000000000000000000000000")
                {
                    return Ok();
                }

                //取得Line Event
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                var responseMsg = "";

                //準備回覆訊息
                if(LineEvent.type.ToLower() == "message" && LineEvent.message.type == "text")
                {
                    responseMsg = $"{  isRock.Template.ChatGPT.CallChatGPT(LineEvent.message.text).choices.FirstOrDefault().text}";
                }
                else if(LineEvent.type.ToLower() == "message")
                {
                    responseMsg = $"收到 event : {LineEvent.type} type: {LineEvent.message.type} ";
                }
                else
                {
                    responseMsg = $"收到 event : {LineEvent.type} ";
                }

                //回覆訊息
                this.ReplyMessage(LineEvent.replyToken, responseMsg);

                //response OK
                return Ok();
            }
            catch(Exception ex)
            {
                //回覆訊息
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);

                //response OK
                return Ok();
            }
        }
    }
}
