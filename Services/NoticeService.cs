using SgkService.Concretes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SgkService.Services
{
   public class NoticeService
    {
         Duyurular _duyurular;


        public async void  CheckNotice()
        {
            string txtYol = System.Configuration.ConfigurationManager.AppSettings["txtYol"];
            string webUrl = System.Configuration.ConfigurationManager.AppSettings["WebUrl"];
            string mailKonu = System.Configuration.ConfigurationManager.AppSettings["mailKonu"];
            string mailIcerik = System.Configuration.ConfigurationManager.AppSettings["mailIcerik"];
            string baslikBaslangic = System.Configuration.ConfigurationManager.AppSettings["baslikBaslangic"];
            string baslikBitis = System.Configuration.ConfigurationManager.AppSettings["baslikBitis"];
            string aliciMail = System.Configuration.ConfigurationManager.AppSettings["aliciMail"];
            int baslikBastanKacKarakterKisalt = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["baslikBastanKacKarakterKisalt"]);
            int baslikSondanKacKarakterKisalt = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["baslikSondanKacKarakterKisalt"]);

            try
            {

                WebRequest baglantiTalebi = HttpWebRequest.Create(webUrl);
                WebResponse gelenCevap = baglantiTalebi.GetResponse();
                StreamReader cevapOku = new StreamReader(gelenCevap.GetResponseStream());

                string kaynakKodlar = cevapOku.ReadToEnd();
                //int icerikBaslangicIndex = kaynakKodlar.IndexOf("<h4>") + 27;
                int icerikBaslangicIndex = kaynakKodlar.IndexOf("<" + baslikBaslangic + ">") + baslikBastanKacKarakterKisalt;
                //int icerikBitisIndex = kaynakKodlar.Substring(icerikBaslangicIndex).IndexOf("</h4>") - 4;
                int icerikBitisIndex = kaynakKodlar.Substring(icerikBaslangicIndex).IndexOf("</" + baslikBitis + ">") - baslikSondanKacKarakterKisalt;
                string baslikTut = kaynakKodlar.Substring(icerikBaslangicIndex, icerikBitisIndex); //başlık oluşturuldu.
                baslikTut = HttpUtility.HtmlDecode(baslikTut); //HTML ile kodlanmış bir dizeyi kodu çözülen bir dizeye dönüştürdü.
                string dosya_dizini = AppDomain.CurrentDomain.BaseDirectory.ToString() + txtYol;
                string kontrolTextFile;
                kontrolTextFile = File.ReadAllText(txtYol); //belirtilen dosyayı oku.
                int ilksayac = 0;
                if (kontrolTextFile == "")
                {
                    ilksayac = 1;
                }
                else ilksayac = 2;
                if (File.Exists(dosya_dizini) == false || kontrolTextFile == "") // dizindeki dosya yoksa
                {
                    CreateTxt(txtYol); //yeni txt oluştur.
                }
                kontrolTextFile = File.ReadAllText(txtYol); //belirtilen dosyayı oku.
                if (baslikTut.Length > 0 || baslikTut == kontrolTextFile)//başlık boş değilse
                {

                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine(baslikTut);

                    if (baslikTut != kontrolTextFile)
                    {

                        List<string> satirlarList = new List<string>();
                        satirlarList.Add(baslikTut);
                        MailSender mailSender = new MailSender();
                        if (ilksayac == 1)
                        {
                           await mailSender.SendMessage(mailKonu, "Duyuru bildirimleri aktif edilmiştir." +
                            " İlk Duyuru: <br/> " + mailIcerik,aliciMail);
                            Console.WriteLine("İlk Duyuru, Mail olarak gönderildi.");
                            TextWrite(satirlarList, txtYol);
                        }
                        else if (ilksayac != 1)
                        {
                          await  mailSender.SendMessage(mailKonu, "Yeni Duyuru! <br/> " + mailIcerik, aliciMail);
                            Console.WriteLine("Yeni Duyuru, Mail olarak gönderildi.");
                            TextWrite(satirlarList, txtYol);

                        }

                    }
                    else
                    {
                        Console.WriteLine("Yeni Bildirim yok.");
                    }

                }
            }
            catch (System.IO.FileNotFoundException)
            {
                CreateTxt(txtYol);
            }
            catch (System.IO.IOException)
            {
                CreateTxt(txtYol);
                
            }
            catch (UnauthorizedAccessException)
            {
                CreateTxt(txtYol);
            }



        }


        public void TextWrite(List<string> yazilacakVeri, string nereyeYazilacak)
        {
            try
            {
                foreach (var item in yazilacakVeri)
                {
                    StreamWriter sw = new StreamWriter(nereyeYazilacak); //dosyaya veriyi yazar
                    sw.Write(item);
                    sw.Close();

                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        public void CreateTxt(string txtAddress)
        {
            try
            {
                FileStream fs = File.Open(txtAddress, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fs.Close();
            }
            catch (System.IO.IOException)
            {
                string kontrolTextFile = File.ReadAllText(txtAddress);
                if (File.Exists(txtAddress) == false || kontrolTextFile == "") // dizindeki dosya yoksa
                {
                    CreateTxt(txtAddress); //yeni txt oluştur.
                }
            }

        }
        



    }

}
