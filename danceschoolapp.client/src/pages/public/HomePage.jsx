import Navbar from './sections/Navbar'
import Hero from './sections/Hero'
import Features from './sections/Features'
import About from './sections/About'
import Modalities from './sections/Modalities'
import Footer from './sections/Footer'
import '../../App.css'
function HomePage() {
    return (
        <>
            <Hero />
            <Features />
            <About />
            <Modalities />
            <Footer />
        </>
    )
}

export default HomePage