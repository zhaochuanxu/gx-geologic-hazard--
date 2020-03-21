
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GU_DATA.Entitys;
using System;
using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.IO;

using PagedList;
using System.Data.Entity;
using System.Net;
using Newtonsoft.Json;
using System.Text;

namespace GU_DATA.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        private int Init()
        {
            string Name = this.User.Identity.Name;
            var user = dbcontext.gd_user.FirstOrDefault(u => u.Name == Name);
            if(user.type=="admin")
            {
                return 0;
            }

           else
            {
                Response.Write("<script>alert('权限不够')</script>");

                return 1; ;
            }
        }

        private GDDBEntities dbcontext = new GDDBEntities();
        // GET: Admin

        /*********首页*******/
        #region
        //登录才可以 
        [Authorize]
        public ActionResult Index()
        {
            if (Init() == 1) { return Redirect("~/Home/Index"); }
            else { return View(); }
        }
        #endregion





        /*********登录*******/
        #region
        public ActionResult Login() { return View(); }



        [HttpPost]
        public ActionResult Login(gd_user m)
        {

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
                Response.Write("<script>alert('输入错误')</script>");
                return View(m);
            }


            //数据校验通过 查看数据库是否存在该用户
            else
            {
                //lamada表达式 匿名函数 参数随便找

                //查询用户否存在  返回的是一个实体 类
                var user = dbcontext.gd_user.FirstOrDefault(u => u.Name == m.Name && u.type == "admin");
                //用户存在

                if (user != null)
                {
                    //如果密码不一致
                    if (user.Password != m.Password)
                    {
                        Response.Write("<script>alert('密码错误')</script>");
                        return View(m);
                    }



                    //如果密码一致
                    else
                    {
                        if (user.type != "admin")
                        {
                            Response.Write("<script>alert('不存在该管理员')</script>");
                            return View(m);
                        }



                        else {
                            // Response.Write("<script>alert('登陆成功')</script>");

                            //FormsAuthentication.SetAuthCookie(m.Name, true); //成功登录且不保存cookies
                            //设置用户的 cookie 的值
                            FormsAuthentication.SetAuthCookie(m.Name, true);


                            Session["TYPE"] = "admin";

                            Response.Write("<script>alert('登陆成功');location='../Admin/Index'</script>");

                            //string idenName = HttpContext.User.Identity.Name;
                            //Response.Write(idenName);

                            return View(m);
                        }
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

        #endregion





        /*********展示用户*******/
        #region

        //用户列表
        [Authorize]
        public ActionResult List_User(int? page)
        {
            GDDBEntities dbcontext = new GDDBEntities();

            var userList = from s in dbcontext.gd_user select s;
            //第几页  
            int pageNumber = page ?? 1;

            //每页显示多少条  
            int pageSize = 5;

            //根据ID升序排序  
            userList = userList.OrderBy(x => x.UID);

            //通过ToPagedList扩展方法进行分页  
            IPagedList<gd_user> userPagedList = userList.ToPagedList(pageNumber, pageSize);

            //将分页处理后的列表传给View 
            return View(userPagedList);



            //return View(dbcontext.gd_user.ToList());
        }

        #endregion


        /*********创建用户*******/
        #region
        //创建用户
        [Authorize]
        //不带参数
        public ActionResult Creat_User()
        {


            //var user = dbcontext.gd_user.FirstOrDefault(u => u.UID == id);




            return View();
        }
        //创建用户


        [Authorize]
        [HttpPost]
        public ActionResult Creat_User(gd_user m)
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
            if (Flage_Name == 1)
            {
                Response.Write("<script>alert('用户已存在,不可注册')</script>");
                return View(m);
            }

            //如果用户名不存在
            else
            {
                Ip ip = new Ip();
                // 获取返回对象
                ip = Get_Ip_Address();
                //注意这个在mODEL


                ModelState.Remove("UID");
                // ModelState.Remove("ID_Name");
                //ModelState.Remove("ID_Number");
                //ModelState.Remove("Phone");
                ModelState.Remove("ConfirmPassword");
                //ModelState.Remove("Address");
                //ModelState.Remove("Email");
                //ModelState.Remove("Name");


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
                    // Response.Write("<script>alert('输入错误')</script>");
                    return View(m);
                }


                else
                {
                    try
                    {

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
                                Address = m.Address,
                                ID_Name = m.ID_Name,
                                Phone = m.Phone,
                                ID_Number = m.ID_Number,
                                Change_times = 0


                            });

                            try
                            {
                                //避开设置了  [Required]字段的验证检查
                                //await((IObjectContextAdapter)dbcontext).ObjectContext.SaveChangesAsync();
                                dbcontext.SaveChanges();

                                Response.Write("<script>alert('注册成功')</script>");
                                return View(m);
                            }

                            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                            {

                                Response.Write(dbEx.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage);
                                return View(m);
                            }
                        }



                    }
                  


                    catch (Exception e)
                    {
                        Response.Write(e);

                        return View(m);

                    }




                }



            }


          

        }


        #endregion




        /*********修改用户*******/
        #region


       //修改用户   只是查找该用户
       [Authorize]
        [HttpGet]
        public ActionResult Edit_User(int id)
        {
            Session["Change_ID"] = id;
          //  Response.Write("地址" + Session["Change_ID"].ToString()+ "\n");
            try //直接跳转过来的页面
            {
                //不需要进行数据校验

                var user = dbcontext.gd_user.FirstOrDefault(u => u.UID == id);
                ViewBag.Name = user.Name;

                return View(user);
            }


            catch
            {
                return View();
            }
        }



        //修改用户 
        [HttpPost]
        //提交数据
        [Authorize]
        public ActionResult Edit_User(gd_user m)
        {
            int ID = (int)Session["Change_ID"];

            gd_user modelNew = dbcontext.gd_user.FirstOrDefault(u => u.UID == ID);
            //跨方法传输ID


            ModelState.Remove("UID");
           // ModelState.Remove("ID_Name");
            //ModelState.Remove("ID_Number");
            ModelState.Remove("Phone");
            //ModelState.Remove("ConfirmPassword");
            ModelState.Remove("Address");
            //ModelState.Remove("Email");
            ModelState.Remove("Name");


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
               // Response.Write("<script>alert('输入错误')</script>");
                return View(m);
            }


            else
            {
                try
                {
                    if (m.Address != null) { modelNew.Address = m.Address; }


                    if (m.Email != null) { modelNew.Email = m.Email; }
                    else { modelNew.Email = m.Email; }
                    modelNew.Change_times = modelNew.Change_times + 1;
                    if (m.Password != null)
                    {
                        if (m.ConfirmPassword != m.Password)
                        {
                            Response.Write("<script>alert('密码不一致')</script>");
                            return View(m);
                        }
                    }
                    if (m.Phone != null) { modelNew.Phone = m.Phone; }
                    if (m.ID_Number != null) { modelNew.ID_Number = m.ID_Number; }
                    if (m.ID_Name != null) { modelNew.ID_Name = m.ID_Name; }
                    //modelNew.ID_Number = m.ID_Number;
                    //3.跟新到数
                }
                catch (Exception e)
                {
                    Response.Write(e);

                }


                try
                {
                    //3.跟新到数据库
                    dbcontext.SaveChanges();
                    Response.Write("<script>alert('更新成功')</script>");
                    return View(m);
                }

                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Response.Write(dbEx.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage);


                }

                return View(m);

            }

            #region
            ////Response.Write("地址"+Judge(m.Address)+"\n");
            //Response.Write("地址" + ID+ "\n");
            ////Response.Write("密码"+Judge(m.Password) + "\n");
            ////Response.Write("姓名"+Judge(m.Name) + "\n");
            ////Response.Write("身份证"+Judge(m.ID_Number) + "\n");

            ////if(Judge(m.ID_Name) ==1)
            ////    if (Judge(m.ID_Name) == 1)
            ////        if (Judge(m.ID_Name) == 1)
            ////            if (Judge(m.ID_Name) == 1)
            ////                if (Judge(m.ID_Name) == 1)
            ////                    if (Judge(m.ID_Name) == 1)


            #endregion

            //Response.Write(m.Email);
            //Response.Write(m.Address);
            //Response.Write(m.ID_Name);
            //Response.Write(m.Email);
            //Response.Write(m.Phone);
            //string Name = this.User.Identity.Name;





        }
        #endregion







        /*********删除用户*******/
        #region

        //删除用户    显示页面
        [Authorize]
         public ActionResult Delete_User(int id)
        {

            //权限不够 返回主页自动退出
            if (Init() == 1)
            {
                Response.Write("<script>alert('权限不够')</script>");
                return View();
            }

            //权限足够

            else
            {


                var user = dbcontext.gd_user.FirstOrDefault(u => u.UID == id);

                ViewBag.Name = user.Name;
                Session["Delete_ID"] = user.UID;
                return View(user);
            }


        }


       [HttpPost]

        //删除用户 真正删除
        [Authorize] 
        public ActionResult Delete_User(string Name,int id)
        {

            //权限不够 返回主页自动退出
            if (Init() == 1)
            {
                Response.Write("<script>alert('权限不够')</script>");
                return View();
            }

            //权限足够

            else
            {
                int ID = (int)Session["Delete_ID"];

                gd_user user = new gd_user() { UID = ID};
                try
                {
                    dbcontext.Entry<gd_user>(user).State = EntityState.Deleted;
                    dbcontext.SaveChanges();

                    Response.Write("<script>alert('删除成功')</script>");

                    return View();


                }
                // UserInfo user = from u in context.UserInfo where u.Id = 343 select u;

                // context.UserInfo.Remove(user);

                // 用Remove（）方法时，必须先从EF中查到才能删除



                //UserInfo user = new UserInfo() { Id = 343 };

                // context.Entry<UserInfo>(user).State = System.Data.EntityState.Deleted;

                // 用这种方法不用先查再删除，其实内部做了查询，推荐用这种；


 
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {

                    Response.Write(dbEx.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage);
                    return View();


                }
            }


        }
        #endregion






        /*********欢迎*******/
        [Authorize]
        public ActionResult Welcome()
        {

            
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


        /********用户详情*******/
        #region
        //用户详情
        [Authorize]
        public ActionResult Detail_User(int id)
        {


            var user = dbcontext.gd_user.FirstOrDefault(u => u.UID == id);

            return View(user);
        }
        #endregion

     








    }




}