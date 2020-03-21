using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MySql.Data;

using MySql;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;


using GU_DATA.Models;
using GU_DATA.Entitys;
using System.Runtime.Remoting.Contexts;



using System.Data.Entity.Validation;



using System.Web.Security;


using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Net;
using System.IO;

//using HtmlAgilityPack;

using System.Web.Mvc.Html;

using Newtonsoft.Json;
using System.Text;

namespace GU_DATA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //如果已经通过验证
            if (Request.IsAuthenticated && this.User.Identity.IsAuthenticated)
            {
                GDDBEntities dbcontext = new GDDBEntities();
                string Name = this.User.Identity.Name;
                var user = dbcontext.gd_user.FirstOrDefault(u => u.Name == Name);
                //用户存在

                if (user != null)
                {
                    //如果一次也没有修改过
                    if (user.Change_times == 0)
                    {
                        return RedirectToAction("Detail", "Account");
                    }


                    else
                    {

                        //初始化页面首先获取ip地址所在的地区
                        Ip ip = new Ip();

                        // 获取返回对象
                        ip = Get_Ip_Address();

                        //设置右上角所在的地区
                        ViewBag.Address = ip.pro + " " + ip.city;

                        ViewBag.Place = ip.pro + " " + ip.city;
                        /// 

                        //写入访问日志

                        //String tempPath = System.Web.HttpContext.Current.Server.MapPath(\Test\test.aspx);

                        // \代表网站根目录

                        //String tempPath = server.MapPath(Test\test.aspx);

                        // 表示当前文件所在的目录

                        string FileName = System.Web.HttpContext.Current.Server.MapPath("/log.log");
                        string log_Content = ip.Time + "  " + ip.ip + "   " + ip.pro + "  " + ip.city + " " + ip.addr + "\n" + "\n";
                        System.IO.File.AppendAllText(FileName, log_Content);

                        //// 第一种方法，太麻烦了
                        // StreamWriter sw = null;
                        // if (!System.IO.File.Exists(FileName))
                        // {
                        //     FileStream fs = new FileStream("log.txt", FileMode.Create, FileAccess.Write);
                        //     sw = new StreamWriter(fs);
                        //     sw.WriteLine("log.txt");
                        //     //记得要关闭！不然里面没有字！
                        //     sw.Close();
                        //     fs.Close();
                        // }
                        // else
                        // {
                        //     sw = System.IO.File.AppendText("log.txt");
                        //     sw.WriteLine(log);
                        //     sw.Close();
                        //     //MessageBox.Show("已经有log文件了!");
                        // }


                    }
                }
                else return View();

            }
 
      
            //也就是还没登陆
            return View();
        }

        public ActionResult About()
        {
            /**
            #region
            string conn;
            conn = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();   //对配置文件的访问
            ViewBag.Message = conn;




            //Response.Write(conn); 获取配置文件的信息 连接信息
            MySqlConnection mysqlCoon = new MySqlConnection(conn);//用创建mysqlConnection对象
            mysqlCoon.Open();//数据库连接打开


            string sqlQuery = "SELECT * FROM user"; //注入语句
            MySqlCommand comm = new MySqlCommand(sqlQuery, mysqlCoon);//创建一个command对象 注入


            MySqlDataReader dr = comm.ExecuteReader();   //读取数据集



            while (dr.Read())
            {
                //Response.Write(dr[0]+"<br/>"); //第一列的为0
                ViewBag.Message = dr[1].ToString();


               



            }
            dr.Close();

            #endregion
            ***/

           

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public Ip Get_Ip_Address() //返回参数是一个Ip类
        {

            #region
            /*
            String Html;
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://ncov.dxy.cn/ncovh5/view/pneumonia");
            Request.Timeout = 20 * 1000;//请求超时。
            Request.AllowAutoRedirect = true; //网页自动跳转。
            Request.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";//伪装成谷歌爬虫。
            Request.Method = "GET"; //获取数据的方法。GET
            //Request.Method = "POST";//POST
            //Request.ContentType = "application/json";上传的格式JSON
            Request.KeepAlive = true; //保持
            HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
            using (StreamReader sReader = new StreamReader(Response.GetResponseStream(), Encoding.UTF8))
            {
                 Html= sReader.ReadToEnd();
            }
            ViewBag.Html = Html;
            ViewBag.Response = Response;
            */
            #endregion

            #region
            /*
            HtmlWeb htmlweb = new HtmlWeb();

            //设置编码
            htmlweb.OverrideEncoding = Encoding.UTF8;

            //设置文档
            HtmlDocument htmlDoc = htmlweb.Load(@"https://ncov.dxy.cn/ncovh5/view/pneumonia");

            HtmlNodeCollection Tree_Nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class ='areaBox___3jZkr themeA___2th0K']");
            if(Tree_Nodes!=null)
            {
                ViewBag.Html = "没找到";
                
            }
            else
            {
                ViewBag.Html = "找到了";   
            }
*/


            #endregion


            #region
            /*
            String url = "https://wp.m.163.com/163/page/news/virus_report/index.html?_nw_=1&_anw_=1";
            //下载网页源代码 

            //第一种方法
            WebClient web = new WebClient();//创建webclient对象
            web.Encoding = System.Text.Encoding.UTF8;//定义对象语言
            string html = web.DownloadString(url);//向一个连接请求资源
            /*第二种方法
             WebClient wc = new WebClient();
            Byte[] pageData = wc.DownloadData("http://m.weather.com.cn/data/101110101.html");
                string rr = Encoding.GetEncoding("utf-8").GetString(pageData);



            //加载源代码，获取文档对象
            //var doc = new HtmlDocument(); doc.LoadHtml(docText);
            //更加xpath获取总的对象，如果不为空，就继续选择dl标签
            //var res = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[6]/div[1]/div[1]/div[3]");
            ViewBag.Html = html;
*/


            #endregion

            #region

            //获取ip所在的地址

            HttpRequest Rps = System.Web.HttpContext.Current.Request;
            //在新会话启动时候
            Session["REMOTE_ADDR"] = Rps.ServerVariables["REMOTE_ADDR"].ToString();  //获得请求机器的ip
            if (Session["REMOTE_ADDR"].ToString().Equals("::1"))
            {
                Session["REMOTE_ADDR"] = "119.27.27.174";
            }

            string Address = Session["REMOTE_ADDR"].ToString();
            //string url = String.Format("http://ip.taobao.com/service/getIpInfo.php?ip={0}", Address);

            //string url = String.Format("http://ip.ws.126.net/ipquery?ip=223.96.50.44", Address);
            string url = String.Format("http://whois.pconline.com.cn/ipJson.jsp?ip={0}&json=true", Address);

            string Html;
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(url);
            Request.Timeout = 20 * 1000;//请求超时。
            Request.AllowAutoRedirect = true; //网页自动跳转。
            Request.UserAgent = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)";//伪装成谷歌爬虫。
            Request.Method = "GET"; //获取数据的方法。GET
            //Request.Method = "POST";//POST
            //Request.ContentType = "application/json";上传的格式JSON
            Request.KeepAlive = true; //保持
            try
            {
                HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
                using (StreamReader sReader = new StreamReader(Response.GetResponseStream(), Encoding.Default))
                {
                    Html = sReader.ReadToEnd();
                }



                //ViewBag.Html = url;
                //ViewBag.Html = Html;


                //注意要先编译一下
                Ip ip = JsonConvert.DeserializeObject<Ip>(Html);
                ip.Time = DateTime.Now;
                //ViewBag.All = ip.city + ip.pro + "   " + ip.ip;
                if (ip.city.ToString() == "" && ip.pro.ToString() == "")//如果地区查找不到
                {
                    ip.pro = " ";
                    ip.city = "未知地区";
                }

                return ip;


            }


            catch
            {
                //如果获取ip地址错误或者用的太平洋网站错误
                Ip ip = new Ip();
                ip.pro = " ";
                ip.city = "地区未知";
                ip.ip = Address;
                return ip;
            }




            // Response.Write("这是会话开始的IP地址" + Session["REMOTE_ADDR"]+"<br/>");
            //Response.Write("这是会话的唯一标识符"+Session["SessionID"]);

            #endregion






            //http://ip.taobao.com/service/getIpInfo.php?ip=223.96.50.44
            //http://ip.taobao.com/service/getIpInfo.php?ip=11.1.1.1

        }


    }
}