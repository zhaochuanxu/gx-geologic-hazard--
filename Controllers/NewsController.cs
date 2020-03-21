using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Text;
using HtmlAgilityPack;
namespace GU_DATA.Controllers
{
    public class NewsController : Controller
    {
        // GET: News

       [Authorize]
        public ActionResult Index()
        {
            //取消初始化Init();

            //只是捕捉不能联网的异常
            #region
            try //如果不可以联网
            {

                HtmlWeb htmlweb = new HtmlWeb();

                //设置编码
                htmlweb.OverrideEncoding = Encoding.UTF8;

                //设置文档
                HtmlDocument htmlDoc = htmlweb.Load(@"http://www.gxdzj.gov.cn/html/zhxx/");


                HtmlNodeCollection Tree_Nodes = htmlDoc.DocumentNode.SelectNodes("//html/body/div[3]/div[2]/ul/li");


                /*获取标题和连接和时间*/

                // //表示从根节点开始查找
                /*对每一个节点*/
                #region
                if (Tree_Nodes != null && Tree_Nodes.Count > 0)
                {
                    //Console.WriteLine("查到了" + Tree_Nodes.Count); 
                    int i = 0;

                    //声明连接
                    StringBuilder news_href = new StringBuilder();
                    //声明标题
                    StringBuilder news_title = new StringBuilder();


                    //声明师姐

                    StringBuilder news_time = new StringBuilder();


                    /*开始遍历每个结点*/
                    #region
                    foreach (HtmlNode item in Tree_Nodes) //遍历每一个li 找去每一个的 a
                    {
                        //设置新闻标题
                        news_title = new StringBuilder("title_").Append(i);
                        new StringBuilder("href_").Append(i);

                        //设置
                        news_href = new StringBuilder("href_").Append(i);
                        HtmlNode Node = item.SelectSingleNode(".//a"); //加个点以当前为祖先结点


                        //查找时间节点
                        HtmlNode Node_2 = item.SelectSingleNode(".//span");
                        news_time = new StringBuilder("date_").Append(i);




                        /*获取链接和标题*/
                        #region
                        if (Node != null)
                        {

                            HtmlAttribute att_href = Node.Attributes["href"];
                            HtmlAttribute att_title = Node.Attributes["title"];

                            if (att_href != null)
                            {

                                //StringBuilder get_herf = new StringBuilder("http://www.gxdzj.gov.cn/").Append(att_href.Value.ToString());
                                //返回链接 标题数据
                                StringBuilder get_href = new StringBuilder("http://www.gxdzj.gov.cn").Append(att_href.Value.ToString());
                                ViewData[news_title.ToString()] = att_title.Value.ToString();
                                ViewData[news_href.ToString()] = get_href.ToString();



                                //ViewData[news_title.ToString()] = "http://www.gxdzj.gov.cn/" + att_href.Value.ToString();
                                //Console.WriteLine(att_href.Value + "  " + att_title.Value);
                            }

                        }
                        #endregion



                        /*获取时间1*/
                        #region

                        if (Node_2 != null)
                        {

                            //全部转化为String 返回前台
                            ViewData[news_time.ToString()] = Node_2.InnerText.ToString();
                        }

                        #endregion






                        i++;// 获取下一标题

                    }
                    #endregion


                }
                #endregion
                /*节点结束*/

                return View();

            }
            #endregion


            ////如果不能联网
            #region
            catch (Exception e)
            {
                string error = e.Message + "也就是您未联网";
                error = string.Format(@"<script type=""text/javascript"">alert(""{0}"")</script>", error);
                Response.Write(error);
                return View();
            }
            #endregion

        }
    }
}