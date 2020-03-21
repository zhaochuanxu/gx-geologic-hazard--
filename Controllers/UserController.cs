using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using GU_DATA.Entitys;
namespace GU_DATA.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();

        }

        [HttpPost]
          public ActionResult Edit(gd_user m)
        {


            ModelState.Remove("UID");
            ModelState.Remove("Name");

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

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




    }
}