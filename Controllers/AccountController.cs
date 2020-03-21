using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GU_DATA.Entitys;
using GU_DATA.Models;

using System.Data;

using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

using System.Web.Mvc.Html;

using Newtonsoft.Json;
using System.Data.Entity.Validation;



using System.Web.Security;
using System.Runtime.Remoting.Contexts;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace GU_DATA.Controllers
{
    public class AccountController : Controller
    {
        private  GDDBEntities dbcontext = new GDDBEntities();
        // GET: Account


        public ActionResult Login()
        {
           
            return View();
        }


        [HttpPost]
        public ActionResult Login(Entitys.gd_user m)
        {

            //初始化页面首先获取ip地址所在的地区
            Ip ip = new Ip(); 
            // 获取返回对象
            ip = Get_Ip_Address();

            //仅仅验证账号和密码 去掉那些验证不能为空的
            ModelState.Remove("UID");
            ModelState.Remove("ID_Name");
            ModelState.Remove("ID_Number");
            ModelState.Remove("Phone");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Address");
            ModelState.Remove("Email");

            //如果数据校解不通过
            if (!ModelState.IsValid)
            {

                var errorList = ModelState.ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                );

                #region
                //ModelState.AddModelError(m.Name, errorList[m.Name]);
                //ModelState.AddModelError(m.Password,errorLis)
                //可以在Controller中写验证来控制，第一个参数 key 第二个参数 提示内容
                //ModelState.AddModelError(m.Name,errors.ToString());
                //ModelState.AddModelError(m.Password, errors.ToString());
                //ModelState.AddModelError(m.ConfirmPassword, errors.ToString());
                #endregion
                return View(m);
            }

            //数据校验通过 查看数据库是否存在该用户
            else
            {
                //lamada表达式 匿名函数 参数随便找
               
                //查询用户否存在  返回的是一个实体 类
                var user = dbcontext.gd_user.FirstOrDefault(u => u.Name == m.Name);
                //用户存在

                if (user != null)
                {
                    //如果密码不一致
                    if(user.Password!=m.Password)
                    {
                        Response.Write("<script>alert('密码错误')</script>");
                        return View(m);
                    }

                    //如果密码一致
                    else
                    {
                    // Response.Write("<script>alert('登陆成功')</script>");

                        //FormsAuthentication.SetAuthCookie(m.Name, true); //成功登录且不保存cookies
                        //设置用户的 cookie 的值
                        FormsAuthentication.SetAuthCookie(m.Name, true);

                        Session["TYPE"] = "normal";
                        Session["User"] = m.Name;

                        Response.Write("<script>alert('登陆成功');location='../Home/Index'</script>");

                        //string idenName = HttpContext.User.Identity.Name;
                        //Response.Write(idenName);

                        return View(m);
                    }
                }
               
                // 用户不存在
                else
                {
                    Response.Write("<script>alert('用户不存在')</script>");

                    return View();
                }
               

                //FirstOrDefault：取序列中满足条件的第一个元素，如果没有元素满足条件，则返回
                //默认值（对于可以为null的对象，默认值为null，对于不能为null的对象，如int，默认值为0） 



            }




        }




        
        public ActionResult Register() { return View(); }




        [HttpPost]
        public ActionResult Register(Entitys.gd_user m)
        {
            
            //获取用户信息
            //初始化页面首先获取ip地址所在的地区
            Ip ip = new Ip();

            // 获取返回对象
            ip = Get_Ip_Address();
            //Response.Write(m.Name + m.Password)


            //仅仅验证账号和密码 去掉那些验证不能为空的
            ModelState.Remove("UID");
            ModelState.Remove("ID_Name");
            ModelState.Remove("ID_Number");
            ModelState.Remove("Phone");

            ModelState.Remove("Address");
            ModelState.Remove("Email");



            //如果数据校解不通过
            if (!ModelState.IsValid)
            {

                //查看全部错误信息
                //使用lababm
                //var errors = from item in ModelState where item.Value.Errors.Any() select item.Key;
                //产生List
                //var errorList = (from item in ModelState.Values
                //                 from error in item.Errors
                //                 select error.ErrorMessage).ToList();

                var errorList = ModelState.ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                );

                #region
                //ModelState.AddModelError(m.Name, errorList[m.Name]);
                //ModelState.AddModelError(m.Password,errorLis)
                //可以在Controller中写验证来控制，第一个参数 key 第二个参数 提示内容
                //ModelState.AddModelError(m.Name,errors.ToString());
                //ModelState.AddModelError(m.Password, errors.ToString());
                //ModelState.AddModelError(m.ConfirmPassword, errors.ToString());
                #endregion
               
                return View(m);
            }

            //数据校验通过 通过插入数据库
            else
            {
                //密码不一致
                if (m.Password != m.ConfirmPassword)
                {
                    {//{
                     //    ModelState.AddModelError(m.Password, "密码不一致");
                     //    ModelState.AddModelError(m.ConfirmPassword, "密码不一致");
                     //    return View(m);
                        //Response.Write(m.Password + "   " + m.ConfirmPassword);
                        Response.Write("<script>alert('密码不一致')</script>");
                        return View(m);
                    }
                }

                //通过
                else
                {
                    //查询数据库是否存在
                    int Flage_Name = 0;//默认用户不存在
                    using (GDDBEntities dbcontext = new GDDBEntities())
                    {
                        //查询是否已经存在了用户
                        if (dbcontext.gd_user.Any(o => o.Name == m.Name))
                        { Flage_Name = 1; }
                        else { }


                        dbcontext.SaveChanges();

                    }


                    //如果用户名已经存在
                    if (Flage_Name==1)
                    {
                        Response.Write("<script>alert('用户已存在,不可注册')</script>");
                        return View("Register", m);
                    }

                    //如果用户名不存在
                    else{
                        //注意这个在mODEL
                        using (GDDBEntities dbcontext = new GDDBEntities())
                        {
                            dbcontext.gd_user.Add(new gd_user()
                            {
                                Name = m.Name,
                                Password = m.Password,
                                CreatDay = DateTime.Now,
                                IP = ip.ip,
                                type = "normal",
                                Ip_address = ip.pro + " " + ip.city,
                                ConfirmPassword = m.ConfirmPassword,
                                Address = ip.pro + " " + ip.city,
                                ID_Name = "新用户",
                                Phone= "18888888888",
                                ID_Number ="000000000000000000",
                                Change_times = 0
                                

                            });

                            try
                            {
                                //避开设置了  [Required]字段的验证检查
                                //await((IObjectContextAdapter)dbcontext).ObjectContext.SaveChangesAsync();
                                dbcontext.SaveChanges();
                            }
  
                            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                            {

                                Response.Write(dbEx.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage);
                               
                            }



                        }
                        //  Response.Write("<script>alert('注册成功');location='../Account/Login'</script>");
                        //登录陈宫

                        Response.Write("<script>alert('注册成功')</script>");
                        return View("Login",m);





                    }


                }


                }


        }



        //需要登录修改  这时候是跳转过来的
        [Authorize]

        //其他页面跳转过来
        public ActionResult Detail()
        {
            string Name = this.User.Identity.Name;
            GDDBEntities dbcontext = new GDDBEntities();
            gd_user modelNew = dbcontext.gd_user.Where(a => a.Name == Name).FirstOrDefault();
            if (modelNew.Change_times == 0)
            {
                Response.Write("<script>alert('第一次登陆请补充信息')</script>");
            }
           
            //Response.Write(Session["User"]);
            return View();
        }






        //需要登录修改   这时候是提交表单的
        [Authorize]
        [HttpPost]  //提交
        public ActionResult Detail(gd_user m)
        {
            ModelState.Remove("UID");
            ModelState.Remove("Name");

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");


            //如果数据校解不通过
            if (!ModelState.IsValid)
            {

                //查看全部错误信息
                //使用lababm
                //var errors = from item in ModelState where item.Value.Errors.Any() select item.Key;
                //产生List
                //var errorList = (from item in ModelState.Values
                //                 from error in item.Errors
                //                 select error.ErrorMessage).ToList();

                var errorList = ModelState.ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                );

                #region
                //ModelState.AddModelError(m.Name, errorList[m.Name]);
                //ModelState.AddModelError(m.Password,errorLis)
                //可以在Controller中写验证来控制，第一个参数 key 第二个参数 提示内容
                //ModelState.AddModelError(m.Name,errors.ToString());
                //ModelState.AddModelError(m.Password, errors.ToString());
                //ModelState.AddModelError(m.ConfirmPassword, errors.ToString());
                #endregion

                return View(m);
            }

            //数据校验通过 通过插入数据库
            else
            {
                GDDBEntities dbcontext = new GDDBEntities();
                //Response.Write(m.Email);
                //Response.Write(m.Address);
                //Response.Write(m.ID_Name);
                //Response.Write(m.Email);
                //Response.Write(m.Phone);
                //string Name = this.User.Identity.Name;
                string Name = this.User.Identity.Name;
                gd_user modelNew = dbcontext.gd_user.Where(a => a.Name == Name).FirstOrDefault();


                try
                {
                    //2.设置修改后的值
                    modelNew.Address = m.Address;
                    modelNew.Change_times = modelNew.Change_times + 1;
                    modelNew.Email = m.Email;
                    modelNew.Phone = m.Phone;
                    modelNew.ID_Name = m.ID_Name;
                    modelNew.ID_Number = m.ID_Number;
                    //3.跟新到数
                }
                catch (Exception e)
                {
                    Response.Write(e);
                }


                #region
                /*          
                          //插入

                          GDDBEntities dbcontext = new GDDBEntities();
                          //假设：网络传一个StudentDTO过来  ，将此DTO转化成  数据库实体
                          // Student student = new Student();
                          //student.Id = 1;// 假设DTO传过来的值，主键必须存在，不然会报错的

                         /*使用entry
                          #region
                          //处理传过来的
                          m.Name = this.User.Identity.Name.ToString();
                          #region

                          //                二、为避免先查询数据库，可以直接将 被修改的实体对象 添加到 EF中管理(此时为附加状态Attached)，并手动设置其为未修改状态(Unchanged)，同时设置被修改的实体对象 的 包装类对象 对应属性为修改状态。
                          //优点：修改前不需要查询数据库。
                          //-----------------------------------------------------
                          ////0.0创建修改的 实体对象
                          //Models.BlogArticle model = new BlogArticle();
                          //                model.AId = 12;
                          //                model.ATitle = "新的数据";
                          //                model.AContent = "新的数据~~~~~";

                          //                //0.1添加到EF管理容器中，并获取 实体对象 的伪包装类对象

                          #endregion

                          /*连接到数据库的某个表
                          DbEntityEntry<gd_user> entry = dbcontext.Entry<gd_user>(m);
                          entry.State = EntityState.Unchanged;
                          // DbEntityEntry<Models.BlogArticle> entry = db.Entry<Models.BlogArticle>(model);

                          //**如果使用 Entry 附加 实体对象到数据容器中，则需要手动 设置 实体包装类的对象 的 状态为 Unchanged**
                          //**如果使用 Attach 就不需要这句
                          //entry.State = System.Data.EntityState.Unchanged;


                          //0.2标识 实体对象 某些属性 已经被修改了
                          entry.Property("Address").IsModified = true;

                          entry.Property("Email").IsModified = true;
                          entry.Property("ID_Number").IsModified = true;
                          entry.Property("ID_Name").IsModified = true;
                          entry.Property("Phone").IsModified = true;

          */

                #endregion
                try
                {
                    //3.跟新到数据库
                    dbcontext.SaveChanges();
                }

                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Response.Write(dbEx.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage);

                }

                Response.Write("<script>alert('修改信息成功')</script>");
                return View();





            }


        }



        public ActionResult Reset() { return View(); }



        [HttpPost]
        public ActionResult Reset(Entitys.gd_user m) 
        {



            return View();
        }






        //获取
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


        public ActionResult Error404()
        {
            return View();
        }
        //注销
        public ActionResult Log_off()
        {
           
                HttpContext.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            
        }



    
    }



    public class Ip
    {

        //ip属性
        /// </summary>var lo="山东省", lc="菏泽市"; var localAddress={city:"菏泽市", province:"山东省"}


        ///</summary> city 所在城市
        public string city { get; set; }

        /// <summary>
        /// regionNames  地区所在名字
        /// </summary>
        public string regionNames { set; get; }

        /// <summary>
        /// ip地区所在省份
        /// </summary>
        public string pro { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string ip { set; get; }


        public DateTime Time { set; get; }
        public string addr { set; get; }
        /// <summary>
        /// 类的默认构造函数
        /// </summary>
        public Ip()
        {

        }

    }
}
