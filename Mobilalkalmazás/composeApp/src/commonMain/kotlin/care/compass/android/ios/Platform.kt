package care.compass.android.ios

interface Platform {
    val name: String
}

expect fun getPlatform(): Platform