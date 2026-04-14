import Navbar from '../components/Navbar'
import Hero from '../components/Hero'
import Features from '../components/Features'
import About from '../components/About'
import Modalities from '../components/Modalities'
import Testimonials from '../components/Testimonials'
import Footer from '../components/Footer'
import '../App.css'
function HomePage() {
    return (
        <>
            <Navbar />
            <Hero />
            <Features />
            <About />
            <Modalities />
            <Testimonials />
            <Footer />
        </>
    )
}

export default HomePage