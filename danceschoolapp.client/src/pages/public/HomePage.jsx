import Navbar from '../../components/Navbar'
import Hero from '../../components/Hero'
import Features from '../../components/Features'
import About from '../../components/About'
import Modalities from '../../components/Modalities'
import Footer from '../../components/Footer'
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