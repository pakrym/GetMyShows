using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;

namespace MyShows
{
    /// <summary>
    /// Тип операции
    /// </summary>
    public enum Operation
    {
        Authentication,
        ShowsList,
        SearchByFileName,
        ViwedEpisodesList,
        UnwatchedEpisodesList,
        NextEpisodesList,
        CheckEpisode,
        UnCheckEpisode,
        SyncWatchedEpisodes,
        SetShowStatuse,
        SetShowRating,
        SetEpisodeRating,
        FavoritesEpisodesList,
        AddRemoveFavorite,
        IgnoredEpisodesList,
        AddRemoveIgnored,
        GetFriendsNews,
        SearchShow,
        ShowInfo,
        GetGenres,
        ShowsRating,
        GetUserProfile
    }

    /// <summary>
    /// Статус сериала
    /// </summary>
    public enum ShowStatus
    {
        /// <summary>
        /// Смотрю
        /// </summary>
        watching,
        /// <summary>
        /// Буду смотреть
        /// </summary>
        later,
        /// <summary>
        /// Перестал смотреть
        /// </summary>
        cancelled,
        /// <summary>
        /// Не смотрю
        /// </summary>
        remove
    }

    /// <summary>
    /// Рейтинг
    /// </summary>
    public enum Rating
    {
        /// <summary>
        /// Не стоит смотреть
        /// </summary>
        bad = 1,
        /// <summary>
        /// Так себе
        /// </summary>
        so_so = 2,
        /// <summary>
        /// Можно посмотреть
        /// </summary>
        can_see = 3,
        /// <summary>
        /// Хорошо
        /// </summary>
        good = 4,
        /// <summary>
        /// Отлично
        /// </summary>
        very_good = 5
    }

    /// <summary>
    /// Пол
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Оба
        /// </summary>
        All,
        /// <summary>
        /// Мужчины
        /// </summary>
        Male,
        /// <summary>
        /// Женщины
        /// </summary>
        Female
    }

    /// <summary>
    /// Класс для использования API v 1.4
    /// сайта http://api.myshows.ru/
    /// </summary>
    public class MyShowsAPI
    {
        private string login = "";
        private string password = "";

        //Куки
        CookieCollection cookies = new CookieCollection();
        
        /// <summary>
        /// Определяет статус операци по коду, который она вернула
        /// </summary>
        /// <param name="code">Код</param>
        /// <param name="operation">Тип операции</param>
        /// <returns>Статус</returns>
        public string GetOperationStatus(int code, Operation operation)
        {
            string result = "Статус неизвестен";
            switch (operation)
            {
                case Operation.Authentication:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Аутентификация произведена"; break;
                        case 403: result = "Имя пользователя или пароль не подошли"; break;
                        case 404: result = "Пустые параметры"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.ViwedEpisodesList:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 401: result = "Требуется авторизация"; break;
                        case 404: result = "Сериал не найден"; break;
                        case 200: result = "Все нормально"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.GetGenres:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.ShowsRating:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 404: result = "Неправильные параметры"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.GetUserProfile:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 404: result = "Пользователь не найден"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.SearchByFileName:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Поиск успешен"; break;
                        case 500: result = "Параметр q отсутствует JSON"; break;
                        case 404: result = "Ничего не найдено"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.ShowInfo:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 404: result = "Сериал не нейден"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.SearchShow:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Поиск успешен"; break;
                        case 500: result = "Параметр q отсутствует JSON"; break;
                        case 404: result = "Ничего не найдено"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.ShowsList:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.FavoritesEpisodesList:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.GetFriendsNews:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.IgnoredEpisodesList:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.CheckEpisode:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        case 404: result = "Эпизод не найден"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.AddRemoveFavorite:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        case 404: result = "Эпизод не найден, неправильные параметры"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.AddRemoveIgnored:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        case 404: result = "Эпизод не найден, неправильные параметры"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.SyncWatchedEpisodes:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        case 404: result = "Сериал не найден"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.SetShowStatuse:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        case 404: result = "Сериал не найден"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.SetShowRating:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        case 404: result = "Сериал не найден, неправильные параметры"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.SetEpisodeRating:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        case 404: result = "Эпизод не найден, неправильные параметры"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.UnCheckEpisode:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        case 404: result = "Эпизод не найден"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.UnwatchedEpisodesList:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        case 404: result = "Ничего не найдено"; break;
                        default:
                            break;
                    }
                    break;
                case Operation.NextEpisodesList:
                    switch (code)
                    {
                        case -1: result = "Операция не была произведена"; break;
                        case 200: result = "Все нормально"; break;
                        case 401: result = "Требуется авторизация"; break;
                        case 404: result = "Ничего не найдено"; break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        //Запрос
        System.Net.WebRequest req = null;

        /// <summary>
        /// Получение MD5-хэша пароля
        /// </summary>
        /// <param name="pass">Пароль</param>
        /// <returns>Хэш MD5</returns>
        public static string GetMD5Pass(string pass)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));
            string result = "";
            foreach (var item in hash)
            {
                result += (item.ToString("X").Length == 1 ? "0" + item.ToString("X") : item.ToString("X")) + " ";
            }
            return result.ToLower().Replace(" ", "");
        }

        /*public int GetRequestCode(HttpStatusCode statusCode)
        {
            int result = -1;
            switch (statusCode)
            {
                case System.Net.HttpStatusCode.Accepted:
                    result = 202;
                    break;
                case System.Net.HttpStatusCode.Ambiguous:
                    result = 300;
                    break;
                case System.Net.HttpStatusCode.BadGateway:
                    result = 502;
                    break;
                case System.Net.HttpStatusCode.BadRequest:
                    result = 400;
                    break;
                case System.Net.HttpStatusCode.Conflict:
                    result = 409;
                    break;
                case System.Net.HttpStatusCode.Continue:
                    result = 100;
                    break;
                case System.Net.HttpStatusCode.Created:
                    result = 201;
                    break;
                case System.Net.HttpStatusCode.ExpectationFailed:
                    result = 417;
                    break;
                case System.Net.HttpStatusCode.Forbidden:
                    result = 403;
                    break;
                case System.Net.HttpStatusCode.Found:
                    result = 302;
                    break;
                case System.Net.HttpStatusCode.GatewayTimeout:
                    result = 504;
                    break;
                case System.Net.HttpStatusCode.Gone:
                    result = 410;
                    break;
                case System.Net.HttpStatusCode.HttpVersionNotSupported:
                    result = 505;
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    result = 500;
                    break;
                case System.Net.HttpStatusCode.LengthRequired:
                    result = 411;
                    break;
                case System.Net.HttpStatusCode.MethodNotAllowed:
                    result = 405;
                    break;
                case System.Net.HttpStatusCode.Moved:
                    result = 301;
                    break;
                case System.Net.HttpStatusCode.NoContent:
                    result = 204;
                    break;
                case System.Net.HttpStatusCode.NonAuthoritativeInformation:
                    result = 203;
                    break;
                case System.Net.HttpStatusCode.NotAcceptable:
                    result = 406;
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    result = 404;
                    break;
                case System.Net.HttpStatusCode.NotImplemented:
                    result = 501;
                    break;
                case System.Net.HttpStatusCode.NotModified:
                    result = 304;
                    break;
                case System.Net.HttpStatusCode.OK:
                    result = 200;
                    break;
                case System.Net.HttpStatusCode.PartialContent:
                    result = 206;
                    break;
                case System.Net.HttpStatusCode.PaymentRequired:
                    result = 402;
                    break;
                case System.Net.HttpStatusCode.PreconditionFailed:
                    result = 412;
                    break;
                case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                    result = 407;
                    break;
                case System.Net.HttpStatusCode.RedirectKeepVerb:
                    result = 307;
                    break;
                case System.Net.HttpStatusCode.RedirectMethod:
                    result = 303;
                    break;
                case System.Net.HttpStatusCode.RequestEntityTooLarge:
                    result = 413;
                    break;
                case System.Net.HttpStatusCode.RequestTimeout:
                    result = 408;
                    break;
                case System.Net.HttpStatusCode.RequestUriTooLong:
                    result = 414;
                    break;
                case System.Net.HttpStatusCode.RequestedRangeNotSatisfiable:
                    result = 416;
                    break;
                case System.Net.HttpStatusCode.ResetContent:
                    result = 205;
                    break;
                case System.Net.HttpStatusCode.ServiceUnavailable:
                    result = 503;
                    break;
                case System.Net.HttpStatusCode.SwitchingProtocols:
                    result = 101;
                    break;
                case System.Net.HttpStatusCode.Unauthorized:
                    result = 401;
                    break;
                case System.Net.HttpStatusCode.UnsupportedMediaType:
                    result = 415;
                    break;
                case System.Net.HttpStatusCode.Unused:
                    result = 306;
                    break;
                case System.Net.HttpStatusCode.UseProxy:
                    result = 305;
                    break;
                default:
                    break;
            }
            return result;
        }*/

        /// <summary>
        /// Создание класса MyShowsAPI
        /// </summary>
        /// <param name="Login">Логин на сайте http://myshows.ru/ </param>
        /// <param name="Password">Пароль на сайте http://myshows.ru/ </param>
        /// <param name="useMD5">В качестве пароля передается строка MD5 </param>
        public MyShowsAPI(string Login, string Password, bool useMD5)
        {
            login = Login;
            if (useMD5)
                password = Password;
            else
                password = GetMD5Pass(Password);
            cookies = new CookieCollection();
        }

        /// <summary>
        /// Авторизация на сервисе http://myshows.ru/
        /// </summary>
        /// <returns>Код ответа</returns>
        public int Authentication()
        {
            int result = -1;
            try
            {

                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/login?login=" + login + "&password=" + password);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
                cookies = (resp as System.Net.HttpWebResponse).Cookies;
                //req. = new Uri("");

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Список сериалов пользователя
        /// </summary>
        /// <param name="data">Данные о сериалах которые вернул сервер</param>
        /// <returns>Код ответа</returns>
        public int ShowsList(ref Hashtable data)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/shows/");
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);

                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Список просмотренных серий
        /// </summary>
        /// <param name="data">Данные о сериях которые вернул сервер</param>
        /// <param name="ShowId">АйДи сериала</param>
        /// <returns>Код ответа</returns>
        public int ViwedEpisodesList(ref Hashtable data, string ShowId)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/shows/" + ShowId + "/");
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);

                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Список непросмотренных серий
        /// </summary>
        /// <param name="data">Данные о прошлых сериях которые вернул сервер</param>
        /// <returns>Код ответа</returns>
        public int UnwatchedEpisodesList(ref Hashtable data)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/episodes/unwatched/");
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);

                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Список будещих серий
        /// </summary>
        /// <param name="data">Данные о будущих сериях которые вернул сервер</param>
        /// <returns>Код ответа</returns>
        public int NextEpisodesList(ref Hashtable data)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/episodes/unwatched/");
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);

                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Отмечание эпизода
        /// </summary>
        /// <param name="EpisodeId">АйДи эпизода</param>
        /// <returns>Код ответа</returns>
        public int CheckEpisode(string EpisodeId)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/episodes/check/" + EpisodeId);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Отмечание эпизода
        /// </summary>
        /// <param name="EpisodeId">АйДи эпизода</param>
        /// <param name="rating">Оценка эпизода (1-5)</param>
        /// <returns>Код ответа</returns>
        public int CheckEpisode(string EpisodeId, Rating rating)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/episodes/check/" + EpisodeId + "?rating=" + ((int)rating).ToString());
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Снятие отметки с эпизода
        /// </summary>
        /// <param name="EpisodeId">АйДи эпизода</param>
        /// <returns>Код ответа</returns>
        public int UnCheckEpisode(string EpisodeId)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/episodes/uncheck/" + EpisodeId);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Синхронизация всех просмотренных эпизодов
        /// </summary>
        /// <param name="ShowId">АйДи сериала</param>
        /// <param name="Episodes">Список АйДи эпизодов через запятую:
        /// 11111,2222,3333,4444
        /// </param>
        /// <returns>Код ответа</returns>
        public int SyncWatchedEpisodes(string ShowId, string Episodes)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/shows/" + ShowId + "/sync?episodes=" + Episodes);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Управление статусом сериала
        /// </summary>
        /// <param name="ShowId">АйДи сериала</param>
        /// <param name="status">Статус сериала</param>
        /// <returns>Код ответа</returns>
        public int SetShowStatuse(string ShowId, ShowStatus status)
        {
            int result = -1;
            try
            {
                string stat = "";
                switch (status)
                {
                    case ShowStatus.watching:
                        stat = "watching";
                        break;
                    case ShowStatus.later:
                        stat = "later";
                        break;
                    case ShowStatus.cancelled:
                        stat = "cancelled";
                        break;
                    case ShowStatus.remove:
                        stat = "remove";
                        break;
                    default:
                        break;
                }
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/shows/" + ShowId + "/" + stat);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Управление рейтингом сериала
        /// </summary>
        /// <param name="ShowId">АйДи сериала</param>
        /// <param name="rating">Рейтинг сериала</param>
        /// <returns>Код ответа</returns>
        public int SetShowRating(string ShowId, Rating rating)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/shows/"+ShowId+"/rate/"+((int)rating).ToString());
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Управление рейтингом эпизода
        /// </summary>
        /// <param name="EpisodeId">АйДи эпизода</param>
        /// <param name="rating">Рейтинг эпизода</param>
        /// <returns>Код ответа</returns>
        public int SetEpisodeRating(string EpisodeId, Rating rating)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/episodes/rate/"+((int)rating).ToString()+"/" + EpisodeId);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Список избранных эпизодов
        /// </summary>
        /// <param name="data">Данные о избранных сериях которые вернул сервер</param>
        /// <returns>Код ответа</returns>
        public int FavoritesEpisodesList(ref Hashtable data)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/episodes/favorites/list/");
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);

                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Добавление эпизода в избранное
        /// </summary>
        /// <param name="EpisodeId">АйДи эпизода</param>
        /// <returns>Код ответа</returns>
        public int AddEpisodeToFavorites(string EpisodeId)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/favorites/add/" + EpisodeId);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Удаление эпизода из избранного
        /// </summary>
        /// <param name="EpisodeId">АйДи эпизода</param>
        /// <returns>Код ответа</returns>
        public int RemoveEpisodeFromFavorites(string EpisodeId)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/favorites/remove/" + EpisodeId);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Список игнорируемых эпизодов
        /// </summary>
        /// <param name="data">Данные о игнорируемых сериях которые вернул сервер</param>
        /// <returns>Код ответа</returns>
        public int IgnoredEpisodesList(ref Hashtable data)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/episodes/ignored/list/");
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);

                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Добавление эпизода в игнорируемые
        /// </summary>
        /// <param name="EpisodeId">АйДи эпизода</param>
        /// <returns>Код ответа</returns>
        public int AddEpisodeToIgnore(string EpisodeId)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/ignored/add/" + EpisodeId);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Удаление эпизода из игнорируемых
        /// </summary>
        /// <param name="EpisodeId">АйДи эпизода</param>
        /// <returns>Код ответа</returns>
        public int RemoveEpisodeFromIgnore(string EpisodeId)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/ignored/remove/" + EpisodeId);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Новости друзей
        /// </summary>
        /// <param name="data">Данные о новостях друзей которые вернул сервер</param>
        /// <returns>Код ответа</returns>
        public int GetFriendsNews(ref Hashtable data)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/news/");
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);

                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Поиск сериала
        /// </summary>
        /// <param name="data">Данные о результатах поиска которые вернул сервер</param>
        /// <param name="Query">Запрос для поиска</param>
        /// <returns>Код ответа</returns>
        public int SearchShow(ref Hashtable data, string Query)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/shows/search/?q=" + Query);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);

                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Поиск эпизодов по имени файла
        /// </summary>
        /// <param name="data">Данные о результатах поиска которые вернул сервер</param>
        /// <param name="Query">Запрос для поиска</param>
        /// <returns>Код ответа</returns>
        public int SearchByFileName(ref Hashtable data, string Query)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/shows/search/file/?q=" + Query);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);

                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Информация о сериале со списком эпизодов
        /// </summary>
        /// <param name="data">Данные о сериале которые вернул сервер</param>
        /// <param name="ShowId">АйДи сериала</param>
        /// <returns>Код ответа</returns>
        public int ShowInfo(ref Hashtable data, string ShowId)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/shows/" + ShowId);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);

                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Список жанров
        /// </summary>
        /// <param name="data">Данные жанрах которые вернул сервер</param>
        /// <returns>Код ответа</returns>
        public int GetGenres(ref Hashtable data)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/genres/");
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);

                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }

            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Рейтинг сериалов
        /// </summary>
        /// <param name="gender">Пол</param>
        /// <param name="data">Данные о рейтинге сериалов</param>
        /// <returns>Код ответа</returns>
        public int ShowsRating(Gender gender, ref Hashtable data)
        {
            int result = -1;
            try
            {
                string gen = "";
                switch (gender)
                {
                    case Gender.All:
                        gen = "all";
                        break;
                    case Gender.Male:
                        gen = "male";
                        break;
                    case Gender.Female:
                        gen = "female";
                        break;
                    default:
                        break;
                }

                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/shows/top/"+gen+"/");
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Профиль текущего пользователя
        /// </summary>
        /// <param name="data">Данные о профиле пользователя</param>
        /// <returns>Код ответа</returns>
        public int GetUserProfile(ref Hashtable data)
        {
            return GetUserProfile(login, ref data);
        }

        /// <summary>
        /// Профиль пользователя
        /// </summary>
        /// <param name="Login">Логин пользователя</param>
        /// <param name="data">Данные о профиле пользователя</param>
        /// <returns>Код ответа</returns>
        public int GetUserProfile(string Login, ref Hashtable data)
        {
            int result = -1;
            try
            {
                req = System.Net.HttpWebRequest.Create(@"http://api.myshows.ru/profile/" + Login);
                req.Proxy = System.Net.HttpWebRequest.GetSystemWebProxy();
                (req as HttpWebRequest).CookieContainer = new CookieContainer();
                (req as HttpWebRequest).CookieContainer.Add(cookies);
                System.Net.WebResponse resp = req.GetResponse();
                result = ((int)(resp as System.Net.HttpWebResponse).StatusCode);
                StreamReader st = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                string json = st.ReadToEnd();

                bool success = false;
                object dat = JSON.JsonDecode(json, ref success);
                st.Close();
                if (success)
                {
                    data = (Hashtable)dat;
                }
            }
            catch (WebException ex)
            {
                try
                {
                    result = (int)(ex.Response as System.Net.HttpWebResponse).StatusCode;
                }
                catch
                {
                }
            }
            return result;
        }
    }
}
