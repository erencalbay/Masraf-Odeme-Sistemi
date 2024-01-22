# Masraf Ödeme Sistemi
Bir şirket özelinde sahada çalışan personeli için masraf kalemlerinin takibi ve yönetimi için bir uygulama talep edilmektedir. Bu uygulama ile saha da çalışan personel masraflarını anında sisteme girebilecek ve işveren bunu aynı zamanda hem takip edip edebilecek hem de vakit kaybetmeden harcamayı onaylayıp personele ödemesini yapabilecektir. Çalışan hem evrak fiş vb. toplamaktan kurtulmuş olacak hem de uzun süre sahada olduğu durumda gecikmeden ödemesini alabilecektir.

Uygulama şirket üzerinde yönetici ve saha personeli olmak üzere 2 farklı rolde hizmet verecektir. Çalışan saha personeli sadece sisteme masraf girişi yapacak ve geri ödeme talep edecektir. Personel mevcut taleplerini görecek ve taleplerinin durumunu takip edebilecektir. Onayda bekleyen taleplerini görebilir ve bunları takip edebilir. Sistem yöneticisi konumunda olan şirket kullanıcıları ise mevcut talepleri görecek ve onları onaylayıp red edebilecektir.

Onayladıkları ödemeler için anında ödeme işlemi banka entegrasyonu ile gerçekleştirilecek olup çalışan hesabına EFT ile ilgili tutar yatırılacaktır. Red olan talepler için bir açıklama alanı girişi sağlanacak ve talep sahibi masraf talebinin neden red olduğunu görecektir. (Patika, Akbank .NET Bootcamp)

## Nasıl Çalıştırılır

Bu projeyi çalıştırmak için Visual Studio 2022 önerilir. Ayrıca .NET 7 kullanıldığı için dotnet araçları ef core ve versiyonlarının buna göre indirilmesi tavsiye edilir.

#### Download Visual Studio 2022
https://visualstudio.microsoft.com/tr/vs/

#### Download Entity Framework Core tools
https://learn.microsoft.com/en-us/ef/core/cli/dotnet

#### Download .NET 7 version
https://dotnet.microsoft.com/en-us/download/dotnet/7.0

#### Projenin Başlatılması İçin Konfigurasyonlar

##### 1 - FileAPI ve WebAPI projelerini başlatma belgesi olarak birlikte seçin.

##### 2 - Eğer kütüphanelerle ilgili sorun yaşıyorsanız lütfen versiyon kontrollerini yapın veya tekrardan indirin.

##### 3 - Docker kurulumu gereklidir, RabbitMQ, Redis ve PostgreSQL kurulumlarını Docker üzerinde yapın.

##### Docker - https://docs.docker.com

##### Redis - https://redis.io/docs/install/install-stack/docker/

##### RabbitMQ - https://www.rabbitmq.com/download.html

##### PostgreSQL - https://hub.docker.com/_/postgres

##### NOT: Lütfen port numaralarını kontrol edin. appsettings.json dosyasını ona göre güncelleyin.

##### 4 - Migrationları database'e init yapmayı unutmayın. "Update-Database", eğer migration hatası alıyorsanız migrationları silin ve tekrardan "add-migration" ile oluşturun.

##### 5 - Docker'da kurulan bütün clusterları ayağı kaldırmayı unutmayın ve 1. adımda belirtilen gibi ayağa kaldırın. 


NOT: Bu adımlardan sonra projeyi indirip VS üzerinden çözümü açarsanız projeye erişebilirsiniz. Ana çalışma projesini http ile açın. Swagger'ın düzgün görüntülenmesi için varsayılan tarayıcı olarak Chrome'u kullanmanızı öneririm.

### Projenin Ekran Görüntüleri
Projenin ekran görüntülerini içeren dokümantasyon dosyasını aşağıdaki linkte bulabilirsiniz.
https://docs.google.com/document/d/1DM8PfOd-xNW58pbgYJvjil5F_43MT8WRlid8uw_ZaTg/edit?usp=sharing
