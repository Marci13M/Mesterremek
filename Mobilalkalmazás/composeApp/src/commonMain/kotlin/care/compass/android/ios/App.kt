package care.compass.android.ios
//Minden külső csomag, bővítmény importálása
import androidx.compose.animation.AnimatedVisibility
import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.PaddingValues
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.layout.widthIn
import androidx.compose.foundation.pager.HorizontalPager
import androidx.compose.foundation.pager.rememberPagerState
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.verticalScroll
import androidx.compose.material.Button
import androidx.compose.material.ButtonDefaults
import androidx.compose.material.MaterialTheme
import androidx.compose.material.Surface
import androidx.compose.material.Text
import androidx.compose.material.contentColorFor
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.text.font.FontStyle
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.AnnotatedString
import androidx.compose.ui.text.buildAnnotatedString
import androidx.compose.ui.text.SpanStyle
import androidx.compose.ui.text.withStyle
import androidx.compose.ui.unit.sp
import androidx.compose.ui.unit.dp
import org.jetbrains.compose.resources.painterResource
import org.jetbrains.compose.resources.painterResource
import org.jetbrains.compose.ui.tooling.preview.Preview
import carecompass.composeapp.generated.resources.Res
import carecompass.composeapp.generated.resources.compose_multiplatform
import carecompass.composeapp.generated.resources.main_pic
import cafe.adriel.voyager.navigator.Navigator
import cafe.adriel.voyager.core.screen.Screen
import cafe.adriel.voyager.navigator.CurrentScreen
import cafe.adriel.voyager.navigator.LocalNavigator
import carecompass.composeapp.generated.resources.babi_levente
import carecompass.composeapp.generated.resources.nyikonyuk_gergo
import carecompass.composeapp.generated.resources.szlonkai_marcell
import org.jetbrains.compose.resources.DrawableResource
import androidx.compose.foundation.pager.PagerState
import androidx.compose.material.Card
import androidx.compose.material.Icon
import androidx.compose.material.IconButton
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.ArrowBack
import carecompass.composeapp.generated.resources.care_compass_logo_white

//Applikáció alapvető function-je, ez kezeli a képernyőket is
@Composable
@Preview
fun App() {
    MaterialTheme {
        Navigator(HomeScreen) {
            CurrentScreen() // Rendereli az aktuális képernyőt
        }
    }
}

// Data class a kép és szöveg sliderhez
data class SlideData(val imageRes: DrawableResource, val text: String)

// ImagesSliderWithText function, lényege a kép és szöveg megjelenítése és cserélése
@Composable
fun ImageSliderWithText(pagerState: PagerState) {
    // Lista ami tartalmazza a képeket, illetve az alattuk található szöveget
    val slides = listOf(
        SlideData(Res.drawable.babi_levente, "Kotlin developer, founder"),
        SlideData(Res.drawable.nyikonyuk_gergo, "FullStack developer, founder"),
        SlideData(Res.drawable.szlonkai_marcell, ".NET developer, founder")
    )

    Column(
        modifier = Modifier.fillMaxSize(),
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        HorizontalPager(
            state = pagerState,
            modifier = Modifier.fillMaxWidth(),
            contentPadding = PaddingValues(horizontal = 64.dp), // Helyet hagyunk a széleken
            pageSpacing = 16.dp // Kis távolság az elemek között
        ) { page ->
            Box( // Képet középre igazítjuk
                modifier = Modifier.fillMaxWidth(),
                contentAlignment = Alignment.Center
            ) {
                // Card használata annak érdekében, hogy a képes egymás közti helyét be lehessen állítani
                Card(
                    shape = RoundedCornerShape(16.dp),
                    elevation = 4.dp,
                    modifier = Modifier
                        .fillMaxWidth(0.8f) // Az egyes képek 80%-os szélességűek, így középen lesznek
                        .height(300.dp)
                ) {
                    Image(
                        painter = painterResource(slides[page].imageRes),
                        contentDescription = "Slider Image $page",
                        modifier = Modifier.fillMaxSize(),
                        contentScale = ContentScale.Crop
                    )
                }
            }
        }
        Spacer(modifier = Modifier.height(8.dp))
        Text(
            text = slides[pagerState.currentPage].text,
            fontSize = 14.sp,
            textAlign = TextAlign.Center,
            modifier = Modifier
                .fillMaxWidth()
                .padding(horizontal = 16.dp)
        )
    }
}

// HomeScreen definíció
object HomeScreen : Screen {
    @Composable
    override fun Content() {
        val navigator = LocalNavigator.current // Navigator elérése

        // A főképernyőn az oszlop a képernyő tetejére kerül a jobb elrendezés érdekében
        Column(
            modifier = Modifier
                .fillMaxSize(),
            verticalArrangement = Arrangement.Top,
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            // Kép formázása, kitöltve a rendelkezésre álló szélességet, illetve belecroppol, hogy jól látszódjon
            Image(
                painter = painterResource(Res.drawable.main_pic), // A helyes kép referencia
                contentDescription = "Demo Kép",
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(0.dp)
                    .height(500.dp),
                contentScale = ContentScale.Crop,
            )

            // Egyszerű spacer, lényege hogy jobban elkülönítse egymástól az elemeket, a program során többször előfordul
            Spacer(modifier = Modifier.height(24.dp))

            // Az első szöveg amivel a felhasználó találkozik, középre van igazítva és h5-ös tipográfiával rendelkezik
            Text(
                text = "Üdvözöljük a CareCompass applikációban!",
                textAlign = TextAlign.Center,
                style = MaterialTheme.typography.h5
            )

            Spacer(modifier = Modifier.height(50.dp))

            // Leíró szöveg, csak középreigazítottan
            Text(
                text = "Jelenleg applikációnk fejlesztés alatt áll, azonban az alábbi gomb segítségével elérhetőek az eddigi funkciók.",
                textAlign = TextAlign.Center
            )

            Spacer(modifier = Modifier.height(50.dp))

            // A gomb aminek a segítségével át tudunk lépni a második képernyőre
            Button(
                onClick = { navigator?.push(SecondScreen) }, // Navigáció a SecondScreen-re
                colors = ButtonDefaults.buttonColors(
                    backgroundColor = Color(red = 31, green = 57, blue = 92), // Gomb háttérszínének a formázása
                    contentColor = Color.White
                ),
                // Gomb formájának a beállítása
                shape = RoundedCornerShape(13.dp),
                modifier = Modifier
                    .size(width = 350.dp, height = 50.dp)
            ) {
                // Gomb szövegének definiálása
                Text("Tovább!")
            }
        }
    }
}

// SecondScreen definíció
object SecondScreen : Screen {
    @Composable
    override fun Content() {
        val navigator = LocalNavigator.current // Navigator elérése
        // Lista a sliderhez és annak adataihoz
        val slides = listOf(
            SlideData(Res.drawable.babi_levente, "Bábi Levente"),
            SlideData(Res.drawable.nyikonyuk_gergo, "Nyikonyuk Gergő"),
            SlideData(Res.drawable.szlonkai_marcell, "Szlonkai Marcell")
        )

        // Slider pillanatnyi állapota
        val pagerState = rememberPagerState(pageCount = { slides.size })
        // Rólunk felirat egy oszlopban, kitöltve a rendelkezésre álló helyet, illetve görgethető
        Column (
            modifier = Modifier
                .fillMaxWidth()
                .verticalScroll(rememberScrollState())
                .fillMaxHeight(),
            verticalArrangement = Arrangement.Top,
            horizontalAlignment = Alignment.CenterHorizontally
        ){
            Spacer(modifier = Modifier.height(20.dp))
            Box(
                modifier = Modifier.fillMaxWidth()
            ) {
                Text(
                    text = "Rólunk",
                    textAlign = TextAlign.Center,
                    style = MaterialTheme.typography.h4,
                    fontWeight = FontWeight.Bold,
                    modifier = Modifier.align(Alignment.Center) // Középre igazítás
                )
                // Iconbutton használata a vissza gombhoz
                IconButton(
                    onClick = { navigator?.push(HomeScreen) },
                    modifier = Modifier
                        .align(Alignment.CenterStart) // Balra igazítás
                        .padding(end = 16.dp)
                ) {
                    Icon(
                        imageVector = Icons.Default.ArrowBack, //Nyíl ikon használata
                        contentDescription = "Vissza",
                        tint = Color(red = 31, green = 57, blue = 92) // Szín a háttérhez igazítva
                    )
                }
            }
            Spacer(modifier = Modifier.height(20.dp))
            // Box használata az elválasztó vonalhoz és az azon található szövegre
            Box(
                modifier = Modifier
                    .fillMaxWidth()
                    .height(50.dp)
                    .background(Color(red = 31, green = 57, blue = 92)),
                contentAlignment = Alignment.Center
            ){
                Text(
                    text = "Célunk",
                    textAlign = TextAlign.Left,
                    style = MaterialTheme.typography.h5,
                    color = Color.White,
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(10.dp)
                )
            }
            Spacer(modifier = Modifier.height(10.dp))
            Column(
                modifier = Modifier
                    .fillMaxWidth(),
                horizontalAlignment = Alignment.CenterHorizontally
            ){
                // A vonat alatt található szöveg egy külön oszlopban a könnyebb formázhatóság érdekében
                Text(
                    text = "A CareCompass célja, hogy minden felhasználójának segítségét nyújtson aki magánegészségügyben szeretne számára leginkább megfelelő szolgáltatást találni. Cégünk megalakuláskor egy volt számunkra a lényeg, megkönnyítsük azoknak az embereknek az életét akik nem tudnak kiigazodni a rengeteg kórház szolgáltatásai közt.",
                    textAlign = TextAlign.Left,
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(start = 18.dp)
                )
            }
            Spacer(modifier = Modifier.height(10.dp))
            Box(
                modifier = Modifier
                    .fillMaxWidth()
                    .height(50.dp)
                    .background(Color(red = 31, green = 57, blue = 92)),
                contentAlignment = Alignment.Center
            ){
                Text(
                    text = "Történetünk",
                    textAlign = TextAlign.Left,
                    style = MaterialTheme.typography.h5,
                    color = Color.White,
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(10.dp)
                )
            }
            Spacer(modifier = Modifier.height(10.dp))
            Column(
                modifier = Modifier
                    .fillMaxWidth(),
                horizontalAlignment = Alignment.CenterHorizontally
            ){
                Text(
                    text = "A CareCompass története nem nyúlik olyan régre vissza. 2024 szeptemberében a 3 alapító tag elhatározta, hogy valami olyat szeretne létrehozni amivel megkönnyítheti az emberek dolgát. Hatalmas lelkesedéssel vágtunk bele ebbe a projektbe, hiszen tudtuk ezzel valami olyat hoznánk létre ami megváltoztathatja az emberek mindennapjait. Nagy igyekezettel fejlesztettük/fejlesztjük a CareCompass-t hiszen tudjuk mennyire fontos felhasználóinknak.",
                    textAlign = TextAlign.Justify,
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(start = 18.dp)
                )
            }
            Spacer(modifier = Modifier.height(10.dp))
            Box(
                modifier = Modifier
                    .fillMaxWidth()
                    .height(50.dp)
                    .background(Color(red = 31, green = 57, blue = 92)),
                contentAlignment = Alignment.Center
            ) {
                // Alapítóink felirat ami mellett megjelenik az éppen képernyőn található személy neve
                Text(
                    text = "Alapítóink - ${slides[pagerState.currentPage].text}",
                    textAlign = TextAlign.Left,
                    style = MaterialTheme.typography.h5,
                    color = Color.White,
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(10.dp)
                )
            }

            Spacer(modifier = Modifier.height(10.dp))

            // Slider meghívása a pagerState átadásával, így tudva pontosan kit kell megjeleníteni
            ImageSliderWithText(pagerState)
            Spacer(modifier = Modifier.height(10.dp))
            // Footer
            Row(
                // Modifier beállításai, hogy kitöltse a szélességet, illetve a háttérszínének beállítása
                modifier = Modifier
                    .fillMaxWidth()
                    .height(100.dp)
                    .background(Color(red = 31, green = 57, blue = 92))
            ) {
                // CareCompass logó, miden fontos beállításával
                Image(
                    painter = painterResource(Res.drawable.care_compass_logo_white),
                    contentDescription = "CareCompass Logo",
                    modifier = Modifier
                        .height(100.dp)
                        .padding(top = 5.dp)
                        .padding(bottom = 5.dp)
                        .weight(1f), // Az Image kitölti az 1:1 arányt
                    contentScale = ContentScale.Fit
                )
                // Szöveg oszlopban, hogy megfelelően helyezkedejenek el, illetve a rendelkezésre álló hely 2/3-ád töltse ki
                Column(
                    modifier = Modifier
                        .weight(2f) // A szöveg több helyet kap, mint a kép
                        .padding(top = 15.dp)
                        .padding(start = 15.dp)
                ) {
                    Text(
                        text = "1014 Budapest, Színház utca 5–7.",
                        color = Color.White
                    )
                    Text(
                        text = "+36204206969",
                        color = Color.White
                    )
                    Text(
                        text = "CareCompass.hu",
                        color = Color.White
                    )
                }
            }
        }
    }
}